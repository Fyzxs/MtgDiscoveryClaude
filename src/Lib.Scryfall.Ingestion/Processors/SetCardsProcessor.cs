using System.Net;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Mappers;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class SetCardsProcessor : ICardProcessor
{
    private readonly ISetCardItemDynamicToExtMapper _mapper;
    private readonly ICosmosScribe _scribe;
    private readonly ILogger _logger;

    public SetCardsProcessor(ILogger logger)
        : this(
            new SetCardItemDynamicToExtMapper(),
            new ScryfallSetCardsScribe(logger),
            logger)
    {
    }

    private SetCardsProcessor(ISetCardItemDynamicToExtMapper mapper, ICosmosScribe scribe, ILogger logger)
    {
        _mapper = mapper;
        _scribe = scribe;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallCard card)
    {
        ScryfallSetCardItemExtEntity setCardItem = await _mapper.Map(card.Data()).ConfigureAwait(false);
        OpResponse<ScryfallSetCardItemExtEntity> response = await _scribe.UpsertAsync(setCardItem).ConfigureAwait(false);

        LogSuccess(card.Data(), response);
        LogFailure(card.Data(), response);
    }

    private void LogSuccess(dynamic card, OpResponse<ScryfallSetCardItemExtEntity> response)
    {
        if (response.IsNotSuccessful()) return;

        _logger.LogSetCardStored((string)card.id);
    }

    private void LogFailure(dynamic card, OpResponse<ScryfallSetCardItemExtEntity> response)
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
