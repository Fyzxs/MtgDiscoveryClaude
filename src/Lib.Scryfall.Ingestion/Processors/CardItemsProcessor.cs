using System.Net;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Mappers;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class CardItemsProcessor : ICardProcessor
{
    private readonly ICardItemDynamicToExtMapper _mapper;
    private readonly ICosmosScribe _scribe;
    private readonly ILogger _logger;

    public CardItemsProcessor(ILogger logger)
        : this(
            new CardItemDynamicToExtMapper(),
            new ScryfallCardItemsScribe(logger),
            logger)
    {
    }

    private CardItemsProcessor(ICardItemDynamicToExtMapper mapper, ICosmosScribe scribe, ILogger logger)
    {
        _mapper = mapper;
        _scribe = scribe;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallCard card)
    {
        ScryfallCardItemExtEntity cardItem = await _mapper.Map(card.Data()).ConfigureAwait(false);
        OpResponse<ScryfallCardItemExtEntity> response = await _scribe.UpsertAsync(cardItem).ConfigureAwait(false);

        LogSuccess(card, response);
        LogFailure(card, response);
    }

    private void LogSuccess(IScryfallCard card, OpResponse<ScryfallCardItemExtEntity> response)
    {
        if (response.IsNotSuccessful()) return;

        _logger.LogCardItemStored(card.Id());
    }

    private void LogFailure(IScryfallCard card, OpResponse<ScryfallCardItemExtEntity> response)
    {
        if (response.IsSuccessful()) return;

        _logger.LogCardItemStoreFailed(card.Id(), response.StatusCode);
    }
}

internal static partial class CardItemsProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Successfully stored card {CardId} in CardItems")]
    public static partial void LogCardItemStored(this ILogger logger, string cardId);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to store card {CardId} in CardItems. Status: {Status}")]
    public static partial void LogCardItemStoreFailed(this ILogger logger, string cardId, HttpStatusCode status);
}
