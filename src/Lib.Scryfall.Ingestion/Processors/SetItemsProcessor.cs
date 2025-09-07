using System.Net;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Mappers;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class SetItemsProcessor : ISetProcessor
{
    private readonly ICosmosScribe _scribe;
    private readonly IScryfallSetToCosmosMapper _mapper;
    private readonly ILogger _logger;

    public SetItemsProcessor(ILogger logger)
        : this(
            new ScryfallSetItemsScribe(logger),
            new ScryfallSetToCosmosMapper(),
            logger)
    {
    }

    private SetItemsProcessor(
        ICosmosScribe scribe,
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

        LogSuccess(set, response);
        LogFailure(set, response);
    }

    private void LogSuccess(IScryfallSet set, OpResponse<ScryfallSetItem> response)
    {
        if (response.IsNotSuccessful()) return;

        _logger.LogSetItemStored(set.Code());
    }

    private void LogFailure(IScryfallSet set, OpResponse<ScryfallSetItem> response)
    {
        if (response.IsSuccessful()) return;

        _logger.LogSetItemStoreFailed(set.Code(), response.StatusCode);
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
