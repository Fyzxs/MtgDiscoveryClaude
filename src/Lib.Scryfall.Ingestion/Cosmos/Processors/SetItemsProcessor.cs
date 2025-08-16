using System.Net;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Entities;
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
        ScryfallSetItem setItem = _mapper.Map(set);
        OpResponse<ScryfallSetItem> response = await _scribe.UpsertAsync(setItem).ConfigureAwait(false);

        if (response.IsSuccessful())
        {
            _logger.LogSetItemStored(set.Code());
        }
        else
        {
            _logger.LogSetItemStoreFailed(set.Code(), response.StatusCode);
        }
    }
}

internal static partial class SetItemsProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully stored set {Code} in SetItems")]
    public static partial void LogSetItemStored(this ILogger logger, string code);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to store set {Code} in SetItems. Status: {Status}")]
    public static partial void LogSetItemStoreFailed(this ILogger logger, string code, HttpStatusCode status);
}
