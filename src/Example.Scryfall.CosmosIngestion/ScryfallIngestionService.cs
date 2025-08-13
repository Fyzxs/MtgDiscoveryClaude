using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Mappers;
using Lib.Scryfall.Ingestion.Cosmos.Operators;
using Microsoft.Extensions.Logging;

namespace Example.Scryfall.CosmosIngestion;

internal sealed class ScryfallIngestionService : IScryfallIngestionService
{
    private readonly IAsyncEnumerable<IScryfallSet> _scryfallSets;
    private readonly IScryfallSetItemsScribe _scribe;
    private readonly IScryfallSetToCosmosMapper _mapper;
    private readonly ILogger _logger;

    public ScryfallIngestionService(
        ILogger logger,
        IAsyncEnumerable<IScryfallSet> scryfallSets,
        IScryfallSetItemsScribe scribe,
        IScryfallSetToCosmosMapper mapper)
    {
        _logger = logger;
        _scryfallSets = scryfallSets;
        _scribe = scribe;
        _mapper = mapper;
    }

    public async Task IngestAllSetsAsync()
    {
        int successCount = 0;
        int errorCount = 0;

        _logger.LogInformation("Starting Scryfall sets ingestion to Cosmos DB (excluding digital sets)");

        await foreach (IScryfallSet set in _scryfallSets.ConfigureAwait(false))
        {
            try
            {
                _logger.LogInformation("Processing set: {Code} - {Name}", set.Code(), set.Name());

                Lib.Scryfall.Ingestion.Cosmos.Entities.ScryfallSetItem cosmosItem = _mapper.Map(set);
                OpResponse<Lib.Scryfall.Ingestion.Cosmos.Entities.ScryfallSetItem> response = await _scribe.UpsertAsync(cosmosItem).ConfigureAwait(false);

                if (response.IsSuccessful())
                {
                    successCount++;
                    _logger.LogInformation("Successfully ingested set {Code}", set.Code());
                }
                else
                {
                    errorCount++;
                    _logger.LogError("Failed to ingest set {Code}. Status: {Status}",
                        set.Code(), response.StatusCode);
                }
            }
            catch (Exception ex) when (ex is TaskCanceledException or HttpRequestException or InvalidOperationException)
            {
                errorCount++;
                _logger.LogError(ex, "Error processing set {Code}", set.Code());
            }
        }

        _logger.LogInformation("Ingestion complete. Success: {Success}, Errors: {Errors}",
            successCount, errorCount);
    }
}
