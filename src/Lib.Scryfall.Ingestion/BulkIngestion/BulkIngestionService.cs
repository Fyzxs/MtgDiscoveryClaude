using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Aggregation;
using Lib.Scryfall.Ingestion.Apis;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Apis.Configuration;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Apis.Pipeline;
using Lib.Scryfall.Ingestion.Configuration;
using Lib.Scryfall.Ingestion.Pipeline;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.BulkIngestion;

public sealed class BulkIngestionService : IBulkIngestionService
{
    private readonly IBulkIngestionOrchestrator _orchestrator;

    public BulkIngestionService(IIngestionDashboard dashboard, ILogger logger)
    {
        IBulkProcessingConfiguration config = new ConfigBulkProcessingConfiguration();

        ISetsPipelineService setsPipeline = new SetsPipelineService(
            new FilteredScryfallSetCollection(logger),
            new ScryfallSetItemsScribe(logger),
            dashboard,
            config);

        IRulingsPipelineService rulingsPipeline = new RulingsPipelineService(
            new RulingsBulkDataFetcher(logger),
            new RulingsAggregator(),
            new ScryfallRulingItemsScribe(logger),
            dashboard,
            config);

        ICardsPipelineService cardsPipeline = new CardsPipelineService(
            new CardsBulkDataFetcher(logger),
            new ScryfallCardItemsScribe(logger),
            dashboard,
            config,
            logger);

        IArtistsPipelineService artistsPipeline = new ArtistsPipelineService(
            dashboard,
            logger);

        ITrigramsPipelineService trigramsPipeline = new TrigramsPipelineService(
            new MonoStateCardNameTrigramAggregator(),
            dashboard,
            logger);

        _orchestrator = new BulkIngestionOrchestrator(dashboard, setsPipeline, rulingsPipeline, cardsPipeline, artistsPipeline, trigramsPipeline, config);
    }

    public async Task IngestBulkDataAsync()
    {
        await _orchestrator.OrchestrateBulkIngestionAsync().ConfigureAwait(false);
    }
}