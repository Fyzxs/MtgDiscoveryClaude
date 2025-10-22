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
using Lib.Aggregator.UserCards;
using Lib.Domain.Cards;
using Lib.Domain.UserCards;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Universal.Configurations;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration;

internal sealed class DataMigrationApplication : ExampleApplication
{
    protected override async Task Execute()
    {
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        ILogger logger = loggerFactory.CreateLogger<DataMigrationApplication>();

        logger.LogInformation("Starting Data Migration Tool");

        MigrationConfiguration migrationConfig = MonoStateConfig.CurrentConfiguration
            .GetSection("MigrationConfiguration")
            .Get<MigrationConfiguration>();

        AzureSqlConfiguration sqlConfig = MonoStateConfig.CurrentConfiguration
            .GetSection("AzureSqlConfiguration")
            .Get<AzureSqlConfiguration>();

        ICollectorDataReader sqlReader = new CollectorDataReader(
            loggerFactory.CreateLogger<CollectorDataReader>(),
            sqlConfig);

        DiscoveryCardGopher cosmosGopher = new DiscoveryCardGopher(
            loggerFactory.CreateLogger<DiscoveryCardGopher>());

        ICardDomainService cardDomainService = new CardDomainService(
            loggerFactory.CreateLogger<CardDomainService>());

        INewSystemCardLookup cardLookup = new NewSystemCardLookup(
            loggerFactory.CreateLogger<NewSystemCardLookup>(),
            cardDomainService);

        IUserCardsAggregatorService userCardsAggregator = new UserCardsAggregatorService(
            loggerFactory.CreateLogger<UserCardsAggregatorService>());

        IUserCardsDomainService userCardsDomain = new UserCardsDomainService(
            loggerFactory.CreateLogger<UserCardsDomainService>(),
            userCardsAggregator);

        IUserCardsEntryService userCardsEntry = new UserCardsEntryService(
            loggerFactory.CreateLogger<UserCardsEntryService>(),
            userCardsDomain);

        INewSystemCardAdder cardAdder = new NewSystemCardAdder(
            loggerFactory.CreateLogger<NewSystemCardAdder>(),
            userCardsEntry);

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

        MigrationResult result = await orchestrator.ExecuteMigrationAsync().ConfigureAwait(false);

        logger.LogInformation(
            "Migration completed. Total: {Total}, Success: {Success}, Not Found: {NotFound}, Errors: {Errors}",
            result.TotalRecords,
            result.SuccessfulMigrations,
            result.CardsNotFound,
            result.OtherErrors);
    }
}
