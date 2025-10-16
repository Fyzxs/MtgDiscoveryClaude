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
using Lib.Aggregator.Cards;
using Lib.Aggregator.UserCards;
using Lib.Domain.Cards;
using Lib.Domain.UserCards;
using Lib.MtgDiscovery.Data;
using Lib.MtgDiscovery.Entry;
using Lib.MtgDiscovery.Entry.Apis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration;

internal sealed class DataMigrationApplication : ExampleApplication
{
    public DataMigrationApplication(IConfiguration configuration, ILoggerFactory loggerFactory)
        : base(configuration, loggerFactory)
    {
    }

    protected override async Task<int> Execute()
    {
        ILogger logger = LoggerFactory.CreateLogger<DataMigrationApplication>();

        logger.LogInformation("Starting Data Migration Tool");

        MigrationConfiguration migrationConfig = Configuration
            .GetSection("MigrationConfiguration")
            .Get<MigrationConfiguration>();

        AzureSqlConfiguration sqlConfig = Configuration
            .GetSection("AzureSqlConfiguration")
            .Get<AzureSqlConfiguration>();

        ICollectorDataReader sqlReader = new CollectorDataReader(
            LoggerFactory.CreateLogger<CollectorDataReader>(),
            sqlConfig);

        DiscoveryCardGopher cosmosGopher = new DiscoveryCardGopher(
            LoggerFactory.CreateLogger<DiscoveryCardGopher>());

        ICardDataCoordinationService cardDataService = new CardDataCoordinationService(
            LoggerFactory.CreateLogger<CardDataCoordinationService>());

        ICardDomainService cardDomainService = new CardDomainService(
            LoggerFactory.CreateLogger<CardDomainService>(),
            cardDataService);

        INewSystemCardLookup cardLookup = new NewSystemCardLookup(
            LoggerFactory.CreateLogger<NewSystemCardLookup>(),
            cardDomainService);

        IUserCardsAggregatorService userCardsAggregator = new UserCardsAggregatorService(
            LoggerFactory.CreateLogger<UserCardsAggregatorService>());

        IUserCardsDomainService userCardsDomain = new UserCardsDomainService(
            LoggerFactory.CreateLogger<UserCardsDomainService>(),
            userCardsAggregator);

        IUserCardsEntryService userCardsEntry = new UserCardsEntryService(
            LoggerFactory.CreateLogger<UserCardsEntryService>(),
            userCardsDomain);

        INewSystemCardAdder cardAdder = new NewSystemCardAdder(
            LoggerFactory.CreateLogger<NewSystemCardAdder>(),
            userCardsEntry);

        ILogger mapperLogger = LoggerFactory.CreateLogger("OldToNewCardMapper");

        IOldFinishMapper finishMapper = new OldFinishMapper(mapperLogger);

        IOldSpecialMapper specialMapper = new OldSpecialMapper(mapperLogger);

        IOldToNewCardMapper cardMapper = new OldToNewCardMapper(
            mapperLogger,
            finishMapper,
            specialMapper);

        IErrorLogger errorLogger = new CsvErrorLogger(
            LoggerFactory.CreateLogger<CsvErrorLogger>(),
            migrationConfig);

        ISuccessLogger successLogger = new CsvSuccessLogger(
            LoggerFactory.CreateLogger<CsvSuccessLogger>(),
            migrationConfig);

        IMigrationProgressTracker progressTracker = new MigrationProgressTracker(
            LoggerFactory.CreateLogger<MigrationProgressTracker>());

        IMigrationOrchestrator orchestrator = new MigrationOrchestrator(
            LoggerFactory.CreateLogger<MigrationOrchestrator>(),
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

        return 0;
    }
}
