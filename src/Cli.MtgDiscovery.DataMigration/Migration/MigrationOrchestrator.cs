using System;
using System.Collections.Generic;
using System.Linq;
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
using Lib.Cosmos.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Itrs;
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
    }

    public async Task<MigrationResult> ExecuteMigrationAsync()
    {
        int totalRecords = await _sqlReader.GetTotalCountAsync(_configuration.SourceCollectorId).ConfigureAwait(false);
        _progressTracker.Initialize(totalRecords);

        IEnumerable<CollectorDataRecord> sqlRecords = await _sqlReader
            .ReadAllAsync(_configuration.SourceCollectorId)
            .ConfigureAwait(false);

        int successCount = 0;
        int notFoundCount = 0;
        int errorCount = 0;

        foreach (CollectorDataRecord sqlRecord in sqlRecords)
        {
            try
            {
                bool success = await ProcessRecordAsync(sqlRecord).ConfigureAwait(false);

                if (success)
                {
                    successCount++;
                }
                else
                {
                    notFoundCount++;
                }
            }
            catch (Exception ex)
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

        MigrationResult result = new()
        {
            TotalRecords = totalRecords,
            SuccessfulMigrations = successCount,
            CardsNotFound = notFoundCount,
            OtherErrors = errorCount
        };

        return result;
    }

    private async Task<bool> ProcessRecordAsync(CollectorDataRecord sqlRecord)
    {
        OpResponse<OldDiscoveryCardExtEntity> cosmosResponse = await _cosmosGopher
            .ReadCardAsync(sqlRecord.CardId)
            .ConfigureAwait(false);

        if (cosmosResponse.IsFailure)
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

        IOperationResponse<ICardItemItrEntity> lookupResponse = await _cardLookup
            .LookupCardByScryfallIdAsync(cosmosCard.body.scryfall_id)
            .ConfigureAwait(false);

        if (lookupResponse.IsFailure)
        {
            MigrationError error = new()
            {
                OldCardId = sqlRecord.CardId,
                ScryfallId = cosmosCard.body.scryfall_id,
                SetId = sqlRecord.SetId,
                ErrorReason = "Card not found in new system"
            };

            await _errorLogger.LogErrorAsync(error).ConfigureAwait(false);
            return false;
        }

        ICardItemItrEntity newSystemCard = lookupResponse.ResponseData;

        IEnumerable<IAddCardToCollectionArgsEntity> addCardEntities = await _cardMapper
            .Map((sqlRecord, cosmosCard, newSystemCard, _configuration.TargetUserId))
            .ConfigureAwait(false);

        foreach (IAddCardToCollectionArgsEntity addCardEntity in addCardEntities)
        {
            IOperationResponse<List<CardItemOutEntity>> addResponse = await _cardAdder
                .AddCardToCollectionAsync(addCardEntity)
                .ConfigureAwait(false);

            if (addResponse.IsFailure)
            {
                MigrationError error = new()
                {
                    OldCardId = sqlRecord.CardId,
                    ScryfallId = cosmosCard.body.scryfall_id,
                    SetId = sqlRecord.SetId,
                    ErrorReason = $"Failed to add card to collection: {addResponse.OuterException?.Message ?? "Unknown error"}"
                };

                await _errorLogger.LogErrorAsync(error).ConfigureAwait(false);
                return false;
            }

            MigrationSuccess success = new()
            {
                OldCardId = sqlRecord.CardId,
                ScryfallId = cosmosCard.body.scryfall_id,
                SetId = sqlRecord.SetId,
                Finish = addCardEntity.AddUserCard.Details.Finish,
                Special = addCardEntity.AddUserCard.Details.Special,
                SetGroupId = addCardEntity.AddUserCard.Details.SetGroupId,
                Count = addCardEntity.AddUserCard.Details.Count
            };

            await _successLogger.LogSuccessAsync(success).ConfigureAwait(false);
        }

        return true;
    }
}
