using System.Net;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Shared.Apis.Models;
using Lib.Shared.Abstractions.Identifiers;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class CardsByNameProcessor : ICardProcessor
{
    private readonly ICardNameGuidGenerator _guidGenerator;
    private readonly ICosmosScribe _scribe;
    private readonly ILogger _logger;

    public CardsByNameProcessor(ILogger logger)
        : this(new CardNameGuidGenerator(), new ScryfallCardsByNameScribe(logger), logger)
    {
    }

    private CardsByNameProcessor(ICardNameGuidGenerator guidGenerator, ICosmosScribe scribe, ILogger logger)
    {
        _guidGenerator = guidGenerator;
        _scribe = scribe;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallCard card)
    {
        // Generate deterministic GUID from the English card name
        CardNameGuid nameGuid = _guidGenerator.GenerateGuid((string)card.Data().name);

        ScryfallCardByName cardByName = new()
        {
            NameGuid = nameGuid.AsSystemType().ToString(),
            Data = card.Data()
        };

        OpResponse<ScryfallCardByName> response = await _scribe.UpsertAsync(cardByName).ConfigureAwait(false);

        LogSuccess(card, nameGuid, response);
        LogFailure(card, nameGuid, response);
    }

    private void LogSuccess(IScryfallCard card, CardNameGuid nameGuid, OpResponse<ScryfallCardByName> response)
    {
        if (response.IsNotSuccessful()) return;

        _logger.LogCardByNameStored(card.Id(), nameGuid.AsSystemType().ToString());
    }

    private void LogFailure(IScryfallCard card, CardNameGuid nameGuid, OpResponse<ScryfallCardByName> response)
    {
        if (response.IsSuccessful()) return;

        _logger.LogCardByNameStoreFailed(card.Id(), nameGuid.AsSystemType().ToString(), response.StatusCode);
    }
}

internal static partial class CardsByNameProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Successfully stored card {CardId} in CardsByName with name GUID {NameGuid}")]
    public static partial void LogCardByNameStored(this ILogger logger, string cardId, string nameGuid);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to store card {CardId} in CardsByName with name GUID {NameGuid}. Status: {Status}")]
    public static partial void LogCardByNameStoreFailed(this ILogger logger, string cardId, string nameGuid, HttpStatusCode status);
}
