using System.Net;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class SetCodeIndexProcessor : ISetCodeIndexProcessor
{
    private readonly ICosmosScribe _scribe;
    private readonly ILogger _logger;

    public SetCodeIndexProcessor(ILogger logger)
        : this(
            new ScryfallSetCodeIndexScribe(logger),
            logger)
    {
    }

    private SetCodeIndexProcessor(
        ICosmosScribe scribe,
        ILogger logger)
    {
        _scribe = scribe;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallSet set)
    {
        ScryfallSetCodeIndexExtEntity indexItem = new()
        {
            SetCode = set.Code(),
            SetId = set.Id()
        };

        OpResponse<ScryfallSetCodeIndexExtEntity> response = await _scribe.UpsertAsync(indexItem).ConfigureAwait(false);

        LogSuccess(set, response);
        LogFailure(set, response);
    }

    private void LogSuccess(IScryfallSet set, OpResponse<ScryfallSetCodeIndexExtEntity> response)
    {
        if (response.IsNotSuccessful()) return;

        _logger.LogSetCodeIndexStored(set.Code());
    }

    private void LogFailure(IScryfallSet set, OpResponse<ScryfallSetCodeIndexExtEntity> response)
    {
        if (response.IsSuccessful()) return;

        _logger.LogSetCodeIndexStoreFailed(set.Code(), response.StatusCode);
    }
}

internal static partial class SetCodeIndexProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully stored set code {Code} in SetCodeIndex")]
    public static partial void LogSetCodeIndexStored(this ILogger logger, string code);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to store set code {Code} in SetCodeIndex. Status: {Status}")]
    public static partial void LogSetCodeIndexStoreFailed(this ILogger logger, string code, HttpStatusCode status);
}
