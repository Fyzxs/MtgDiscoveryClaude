using System;
using System.Threading.Tasks;
using Cli.MtgDiscovery.DataMigration.Configuration;
using Cli.MtgDiscovery.DataMigration.ErrorTracking;
using Cli.MtgDiscovery.DataMigration.Mapping;
using Cli.MtgDiscovery.DataMigration.Migration;
using Cli.MtgDiscovery.DataMigration.NewSystem;
using Cli.MtgDiscovery.DataMigration.OldSystem.AzureSql;
using Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Operators;
using Cli.MtgDiscovery.DataMigration.SuccessTracking;
using Example.Core;
using Lib.Aggregator.UserCards.Apis;
using Lib.Domain.Cards.Apis;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Commands;
using Lib.Universal.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration;

internal sealed class DataMigrationApplication : ExampleApplication
{
    protected override async Task Execute()
    {
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        ILogger logger = loggerFactory.CreateLogger<DataMigrationApplication>();

        try
        {
            logger.LogInformation("Starting Data Migration Tool");

            logger.LogDebug("Checking if configuration is available...");
            if (MonoStateConfig.CurrentConfiguration is null)
            {
                logger.LogError("MonoStateConfig.CurrentConfiguration is null!");
                throw new InvalidOperationException("Configuration was not initialized");
            }

            logger.LogDebug("Attempting to load MigrationConfiguration section...");
            IConfigurationSection migrationSection = MonoStateConfig.CurrentConfiguration.GetSection("MigrationConfiguration");

            logger.LogDebug("MigrationConfiguration section exists: {Exists}", migrationSection.Exists());
            logger.LogDebug("MigrationConfiguration section path: {Path}", migrationSection.Path);
            logger.LogDebug("MigrationConfiguration section key: {Key}", migrationSection.Key);

            // Log all children
            foreach (IConfigurationSection child in migrationSection.GetChildren())
            {
                logger.LogDebug("MigrationConfiguration child: {Key} = {Value}", child.Key, child.Value);
            }

            MigrationConfiguration migrationConfig = migrationSection.Get<MigrationConfiguration>();

            AzureSqlConfiguration sqlConfig = MonoStateConfig.CurrentConfiguration
                .GetSection("AzureSqlConfiguration")
                .Get<AzureSqlConfiguration>();

            if (migrationConfig is null || sqlConfig is null)
            {
                logger.LogError("Configuration is missing");
                throw new InvalidOperationException("Configuration is missing");
            }

            logger.LogInformation("Configuration loaded successfully");
            logger.LogInformation("Source Collector ID: '{CollectorId}' (Length: {Length})", migrationConfig.SourceCollectorId, migrationConfig.SourceCollectorId?.Length ?? 0);
            logger.LogInformation("Target User ID: '{UserId}' (Length: {Length})", migrationConfig.TargetUserId, migrationConfig.TargetUserId?.Length ?? 0);
            logger.LogInformation("Error Output Path: '{Path}'", migrationConfig.ErrorOutputPath);
            logger.LogInformation("Success Output Path: '{Path}'", migrationConfig.SuccessOutputPath);
            logger.LogDebug("SQL Connection String: {ConnectionString}", sqlConfig.ConnectionString.Substring(0, Math.Min(50, sqlConfig.ConnectionString.Length)) + "...");

            if (string.IsNullOrWhiteSpace(migrationConfig.SourceCollectorId))
            {
                logger.LogError("SourceCollectorId is null or empty in configuration");
                throw new InvalidOperationException("SourceCollectorId must be configured in MigrationConfiguration section");
            }

            if (string.IsNullOrWhiteSpace(migrationConfig.TargetUserId))
            {
                logger.LogError("TargetUserId is null or empty in configuration");
                throw new InvalidOperationException("TargetUserId must be configured in MigrationConfiguration section");
            }

            logger.LogInformation("Creating SQL data reader...");
            ICollectorDataReader sqlReader = new CollectorDataReader(
                loggerFactory.CreateLogger<CollectorDataReader>(),
                sqlConfig);

            logger.LogInformation("Creating Cosmos DB gopher...");
            DiscoveryCardGopher cosmosGopher = new DiscoveryCardGopher(
                loggerFactory.CreateLogger<DiscoveryCardGopher>());

            logger.LogInformation("Creating card domain service...");
            ICardDomainService cardDomainService = new CardDomainService(
                loggerFactory.CreateLogger<CardDomainService>());

            logger.LogInformation("Creating card lookup service...");
            INewSystemCardLookup cardLookup = new NewSystemCardLookup(
                loggerFactory.CreateLogger<NewSystemCardLookup>(),
                cardDomainService);

            logger.LogInformation("Creating user cards aggregator...");
            IUserCardsAggregatorService userCardsAggregator = new UserCardsAggregatorService(
                loggerFactory.CreateLogger<UserCardsAggregatorService>());

            logger.LogInformation("Creating entry service...");
            IEntryService entryService = new EntryService(loggerFactory.CreateLogger<EntryService>());

            logger.LogInformation("Creating card adder...");
            INewSystemCardAdder cardAdder = new NewSystemCardAdder(
                loggerFactory.CreateLogger<NewSystemCardAdder>(),
                entryService);

            logger.LogInformation("Creating mappers and loggers...");
            ILogger mapperLogger = loggerFactory.CreateLogger("OldToNewCardMapper");

            IOldFinishMapper finishMapper = new OldFinishMapper(mapperLogger);
            IOldSpecialMapper specialMapper = new OldSpecialMapper(mapperLogger);
            IOldToNewCardMapper cardMapper = new OldToNewCardMapper(
                mapperLogger,
                finishMapper,
                specialMapper);

            IErrorLogger errorLogger = new CsvErrorLogger(
                loggerFactory.CreateLogger<CsvErrorLogger>(),
                migrationConfig);

            ISuccessLogger successLogger = new CsvSuccessLogger(
                loggerFactory.CreateLogger<CsvSuccessLogger>(),
                migrationConfig);

            IMigrationProgressTracker progressTracker = new MigrationProgressTracker(
                loggerFactory.CreateLogger<MigrationProgressTracker>());

            logger.LogInformation("Creating migration orchestrator...");
            IMigrationOrchestrator orchestrator = new MigrationOrchestrator(
                loggerFactory.CreateLogger<MigrationOrchestrator>(),
                migrationConfig,
                sqlReader,
                cosmosGopher,
                cardLookup,
                cardAdder,
                cardMapper,
                errorLogger,
                successLogger,
                progressTracker);

            logger.LogInformation("Starting migration execution...");
            MigrationResult result = await orchestrator.ExecuteMigrationAsync().ConfigureAwait(false);

            logger.LogInformation(
                "Migration completed. Total: {Total}, Success: {Success}, Not Found: {NotFound}, Errors: {Errors}",
                result.TotalRecords,
                result.SuccessfulMigrations,
                result.CardsNotFound,
                result.OtherErrors);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error during migration execution");
            logger.LogError("Exception Type: {Type}", ex.GetType().FullName);
            logger.LogError("Exception Message: {Message}", ex.Message);
            logger.LogError("Stack Trace: {StackTrace}", ex.StackTrace);

            if (ex.InnerException != null)
            {
                logger.LogError("Inner Exception Type: {Type}", ex.InnerException.GetType().FullName);
                logger.LogError("Inner Exception Message: {Message}", ex.InnerException.Message);
            }

            throw;
        }
    }
}
