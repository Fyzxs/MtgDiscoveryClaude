using System.Net;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Cosmos.Entities;
using Lib.Scryfall.Ingestion.Cosmos.Mappers;
using Lib.Scryfall.Ingestion.Cosmos.Operators;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class SetCardsProcessor : ICardsProcessor
{
    private readonly IScryfallCardToSetCardMapper _mapper;
    private readonly ICosmosScribe _scribe;
    private readonly ILogger _logger;

    public SetCardsProcessor(ILogger logger)
        : this(
            new ScryfallCardToSetCardMapper(),
            new ScryfallSetCardsScribe(logger),
            logger)
    {
    }

    private SetCardsProcessor(IScryfallCardToSetCardMapper mapper, ICosmosScribe scribe, ILogger logger)
    {
        _mapper = mapper;
        _scribe = scribe;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallCard card)
    {
        ScryfallSetCard setCard = _mapper.Map(card.Data());
        OpResponse<ScryfallSetCard> response = await _scribe.UpsertAsync(setCard).ConfigureAwait(false);

        LogSuccess(card.Data(), response);
        LogFailure(card.Data(), response);
    }

    private void LogSuccess(dynamic card, OpResponse<ScryfallSetCard> response)
    {
        if (response.IsNotSuccessful()) return;

        _logger.LogSetCardStored((string)card.id);
    }

    private void LogFailure(dynamic card, OpResponse<ScryfallSetCard> response)
    {
        if (response.IsSuccessful()) return;

        _logger.LogSetCardStoreFailed((string)card.id, response.StatusCode);
    }
}

internal static partial class SetCardsProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Successfully stored card {CardId} in SetCards")]
    public static partial void LogSetCardStored(this ILogger logger, string cardId);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to store card {CardId} in SetCards. Status: {Status}")]
    public static partial void LogSetCardStoreFailed(this ILogger logger, string cardId, HttpStatusCode status);
}
