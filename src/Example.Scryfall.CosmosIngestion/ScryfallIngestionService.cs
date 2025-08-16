using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Lib.BlobStorage.Apis;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Mappers;
using Lib.Scryfall.Ingestion.Cosmos.Operators;
using Lib.Scryfall.Ingestion.Cosmos.Processors;
using Lib.Scryfall.Ingestion.Icons;
using Lib.Scryfall.Ingestion.Icons.Processors;
using Microsoft.Extensions.Logging;

namespace Example.Scryfall.CosmosIngestion;

internal sealed class ScryfallIngestionService : IScryfallIngestionService
{
    private readonly IAsyncEnumerable<IScryfallSet> _scryfallSets;
    private readonly ISetItemsProcessor _setItemsProcessor;
    private readonly ISetAssociationsProcessor _setAssociationsProcessor;
    private readonly ISetIconProcessor _setIconProcessor;
    private readonly ILogger _logger;

    public ScryfallIngestionService(ILogger logger)
        : this(
            logger,
            new NonDigitalScryfallSetCollection(logger),
            new SetItemsProcessor(
                new ScryfallSetItemsScribe(logger),
                new ScryfallSetToCosmosMapper(),
                logger),
            new SetAssociationsProcessor(
                new ScryfallSetAssociationsScribe(logger),
                new ScryfallSetToAssociationMapper(),
                logger),
            new SetIconProcessor(
                new SetIconDownloader(logger),
                new SetIconBlobScribe(
                    logger,
                    new SetIconContainerAdapter(
                        logger,
                        new SetIconContainerDefinition(),
                        new ServiceLocatorAuthBlobConnectionConfig())),
                logger))
    {
    }

    private ScryfallIngestionService(
        ILogger logger,
        IAsyncEnumerable<IScryfallSet> scryfallSets,
        ISetItemsProcessor setItemsProcessor,
        ISetAssociationsProcessor setAssociationsProcessor,
        ISetIconProcessor setIconProcessor)
    {
        _logger = logger;
        _scryfallSets = scryfallSets;
        _setItemsProcessor = setItemsProcessor;
        _setAssociationsProcessor = setAssociationsProcessor;
        _setIconProcessor = setIconProcessor;
    }

    public async Task IngestAllSetsAsync()
    {
        int processedCount = 0;

        _logger.LogInformation("Starting Scryfall sets ingestion to Cosmos DB (excluding digital sets)");

        await foreach (IScryfallSet set in _scryfallSets.ConfigureAwait(false))
        {
            try
            {
                _logger.LogInformation("Processing set: {Code} - {Name}", set.Code(), set.Name());

                await _setItemsProcessor.ProcessAsync(set).ConfigureAwait(false);
                await _setAssociationsProcessor.ProcessAsync(set).ConfigureAwait(false);
                await _setIconProcessor.ProcessAsync(set).ConfigureAwait(false);

                processedCount++;
            }
            catch (Exception ex) when (ex is TaskCanceledException or HttpRequestException or InvalidOperationException)
            {
                _logger.LogError(ex, "Error processing set {Code}", set.Code());
            }
        }

        _logger.LogInformation("Ingestion complete. Processed {Count} sets", processedCount);
    }
}
