using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Mappers;
using Lib.Scryfall.Ingestion.Cosmos.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Cosmos.Processors;

internal sealed class SetItemsProcessor : ISetItemsProcessor
{
    private readonly IScryfallSetItemsScribe _scribe;
    private readonly IScryfallSetToCosmosMapper _mapper;
    private readonly ILogger _logger;

    public SetItemsProcessor(
        IScryfallSetItemsScribe scribe,
        IScryfallSetToCosmosMapper mapper,
        ILogger logger)
    {
        _scribe = scribe;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallSet set)
    {
        Entities.ScryfallSetItem setItem = _mapper.Map(set);
        Lib.Cosmos.Apis.Operators.OpResponse<Entities.ScryfallSetItem> response = await _scribe.UpsertAsync(setItem).ConfigureAwait(false);

        if (response.IsSuccessful())
        {
#pragma warning disable CA1848 // Use LoggerMessage delegates
            _logger.LogInformation("Successfully stored set {Code} in SetItems", set.Code());
#pragma warning restore CA1848
        }
        else
        {
#pragma warning disable CA1848 // Use LoggerMessage delegates
            _logger.LogError("Failed to store set {Code} in SetItems. Status: {Status}",
                set.Code(), response.StatusCode);
#pragma warning restore CA1848
        }
    }
}
