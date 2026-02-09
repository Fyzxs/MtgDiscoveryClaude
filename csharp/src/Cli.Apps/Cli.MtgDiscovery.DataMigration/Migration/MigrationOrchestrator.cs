using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cli.MtgDiscovery.DataMigration.Configuration;
using Cli.MtgDiscovery.DataMigration.ErrorTracking;
using Cli.MtgDiscovery.DataMigration.Mapping;
using Cli.MtgDiscovery.DataMigration.NewSystem;
using Cli.MtgDiscovery.DataMigration.OldSystem.AzureSql;
using Cli.MtgDiscovery.DataMigration.OldSystem.AzureSql.Entities;
using Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Entities;
using Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Operators;
using Cli.MtgDiscovery.DataMigration.SuccessTracking;
using Lib.Cosmos.Apis.Operators;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.Migration;

internal sealed class MigrationOrchestrator : IMigrationOrchestrator
{
    private readonly ILogger _logger;
    private readonly MigrationConfiguration _configuration;
    private readonly ICollectorDataReader _sqlReader;
    private readonly DiscoveryCardGopher _cosmosGopher;
    private readonly INewSystemCardLookup _cardLookup;
    private readonly INewSystemCardAdder _cardAdder;
    private readonly IOldToNewCardMapper _cardMapper;
    private readonly IErrorLogger _errorLogger;
    private readonly ISuccessLogger _successLogger;
    private readonly IMigrationProgressTracker _progressTracker;
    private readonly UserSetCardsRecalculator _userSetCardsRecalculator;

    public MigrationOrchestrator(
        ILogger logger,
        MigrationConfiguration configuration,
        ICollectorDataReader sqlReader,
        DiscoveryCardGopher cosmosGopher,
        INewSystemCardLookup cardLookup,
        INewSystemCardAdder cardAdder,
        IOldToNewCardMapper cardMapper,
        IErrorLogger errorLogger,
        ISuccessLogger successLogger,
        IMigrationProgressTracker progressTracker)
    {
        _logger = logger;
        _configuration = configuration;
        _sqlReader = sqlReader;
        _cosmosGopher = cosmosGopher;
        _cardLookup = cardLookup;
        _cardAdder = cardAdder;
        _cardMapper = cardMapper;
        _errorLogger = errorLogger;
        _successLogger = successLogger;
        _progressTracker = progressTracker;
        _userSetCardsRecalculator = new UserSetCardsRecalculator(logger);
    }

    public async Task<MigrationResult> ExecuteMigrationAsync()
    {
        int totalRecords = await _sqlReader.GetTotalCountAsync(_configuration.SourceCollectorId).ConfigureAwait(false);
        _progressTracker.Initialize(totalRecords);

        IEnumerable<CollectorDataRecord> sqlRecords = await _sqlReader
            .ReadAllAsync(_configuration.SourceCollectorId)
            .ConfigureAwait(false);

        // Log test mode if configured
        if (string.IsNullOrWhiteSpace(_configuration.TestSetCode) is false)
        {
            _logger.LogWarning("TEST MODE: Only migrating cards from set code '{SetCode}'", _configuration.TestSetCode);
        }

        int successCount = 0;
        int skippedCount = 0;
        int notFoundCount = 0;
        int errorCount = 0;

        foreach (CollectorDataRecord sqlRecord in sqlRecords)
        {
            try
            {
                bool? processResult = await ProcessRecordAsync(sqlRecord).ConfigureAwait(false);

                if (processResult is null)
                {
                    skippedCount++;
                }
                else if (processResult.Value)
                {
                    successCount++;
                }
                else
                {
                    notFoundCount++;
                }
            }
            catch (Exception ex) when (ex is not OutOfMemoryException && ex is not StackOverflowException)
            {
                errorCount++;
                _logger.LogError(ex, "Error processing record {CardId}", sqlRecord.CardId);

                MigrationError error = new()
                {
                    OldCardId = sqlRecord.CardId,
                    ScryfallId = string.Empty,
                    SetId = sqlRecord.SetId,
                    ErrorReason = ex.Message
                };

                await _errorLogger.LogErrorAsync(error).ConfigureAwait(false);
            }

            _progressTracker.IncrementProgress();
        }

        await _errorLogger.FlushAsync().ConfigureAwait(false);
        await _successLogger.FlushAsync().ConfigureAwait(false);
        _progressTracker.Complete();

        if (skippedCount > 0)
        {
            _logger.LogInformation("Skipped {SkippedCount} records (not matching test set code)", skippedCount);
        }

        // Recalculate UserSetCards totals from UserCards data
        _logger.LogInformation("Recalculating UserSetCards totals...");
        await _userSetCardsRecalculator.RecalculateAsync(_configuration.TargetUserId).ConfigureAwait(false);

        MigrationResult result = new()
        {
            TotalRecords = totalRecords,
            SuccessfulMigrations = successCount,
            CardsNotFound = notFoundCount,
            OtherErrors = errorCount
        };

        return result;
    }

    private async Task<bool?> ProcessRecordAsync(CollectorDataRecord sqlRecord)
    {
        OpResponse<OldDiscoveryCardExtEntity> cosmosResponse = await _cosmosGopher
            .ReadCardAsync(sqlRecord.CardId)
            .ConfigureAwait(false);

        if (cosmosResponse.IsNotSuccessful())
        {
            MigrationError error = new()
            {
                OldCardId = sqlRecord.CardId,
                ScryfallId = string.Empty,
                SetId = sqlRecord.SetId,
                ErrorReason = "Card not found in old Cosmos database"
            };

            await _errorLogger.LogErrorAsync(error).ConfigureAwait(false);
            return false;
        }

        OldDiscoveryCardExtEntity cosmosCard = cosmosResponse.Value;

        IOperationResponse<ICardItemOufEntity> lookupResponse = await _cardLookup
            .LookupCardByScryfallIdAsync(cosmosCard.Body.ScryfallId)
            .ConfigureAwait(false);

        if (lookupResponse.IsFailure)
        {
            MigrationError error = new()
            {
                OldCardId = sqlRecord.CardId,
                ScryfallId = cosmosCard.Body.ScryfallId,
                SetId = sqlRecord.SetId,
                ErrorReason = "Card not found in new system"
            };

            await _errorLogger.LogErrorAsync(error).ConfigureAwait(false);
            return false;
        }

        ICardItemOufEntity newSystemCard = lookupResponse.ResponseData;

        // Skip if test set code is configured and doesn't match
        if (string.IsNullOrWhiteSpace(_configuration.TestSetCode) is false &&
            string.Equals(newSystemCard.SetCode, _configuration.TestSetCode, StringComparison.OrdinalIgnoreCase) is false)
        {
            return null; // Skipped
        }

        IEnumerable<IAddCardToCollectionArgsEntity> addCardEntities = await _cardMapper
            .Map((sqlRecord, cosmosCard, newSystemCard, _configuration.TargetUserId, _configuration.ReplaceExistingCounts))
            .ConfigureAwait(false);

        // Add each variation to UserCards and UserSetCards (totals recalculated at end)
        foreach (IAddCardToCollectionArgsEntity addCardEntity in addCardEntities)
        {
            IOperationResponse<List<CardItemOutEntity>> addResponse = await _cardAdder
                .AddCardToCollectionAsync(addCardEntity, CancellationToken.None)
                .ConfigureAwait(false);

            if (addResponse.IsFailure)
            {
                MigrationError error = new()
                {
                    OldCardId = sqlRecord.CardId,
                    ScryfallId = cosmosCard.Body.ScryfallId,
                    SetId = sqlRecord.SetId,
                    ErrorReason = $"Failed to add user card: {addResponse.OuterException?.Message ?? "Unknown error"}"
                };

                await _errorLogger.LogErrorAsync(error).ConfigureAwait(false);
                return false;
            }

            MigrationSuccess success = new()
            {
                OldCardId = sqlRecord.CardId,
                ScryfallId = cosmosCard.Body.ScryfallId,
                SetId = sqlRecord.SetId,
                Finish = addCardEntity.AddUserCard.UserCardDetails.Finish,
                Special = addCardEntity.AddUserCard.UserCardDetails.Special,
                SetGroupId = addCardEntity.AddUserCard.UserCardDetails.SetGroupId,
                Count = addCardEntity.AddUserCard.UserCardDetails.Count
            };

            await _successLogger.LogSuccessAsync(success).ConfigureAwait(false);
        }

        return true;
    }
}
