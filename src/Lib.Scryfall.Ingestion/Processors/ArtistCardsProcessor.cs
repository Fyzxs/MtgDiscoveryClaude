using System.Net;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class ArtistCardsProcessor : ICardProcessor
{
    private readonly ICosmosScribe _scribe;
    private readonly ILogger _logger;

    public ArtistCardsProcessor(ILogger logger)
        : this(
            new ScryfallArtistCardsScribe(logger),
            logger)
    {
    }

    private ArtistCardsProcessor(ICosmosScribe scribe, ILogger logger)
    {
        _scribe = scribe;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallCard card)
    {
        foreach (string artistId in card.ArtistIds())
        {
            await ProcessArtistCard(artistId, card).ConfigureAwait(false);
        }
    }

    private async Task ProcessArtistCard(string artistId, IScryfallCard card)
    {
        ScryfallArtistCardExtEntity artistCard = new()
        {
            ArtistId = artistId,
            Data = card.Data()
        };
        OpResponse<ScryfallArtistCardExtEntity> response = await _scribe.UpsertAsync(artistCard).ConfigureAwait(false);

        LogSuccess(artistId, card, response);
        LogFailure(artistId, card, response);
    }

    private void LogSuccess(string artistId, IScryfallCard card, OpResponse<ScryfallArtistCardExtEntity> response)
    {
        if (response.IsNotSuccessful()) return;

        _logger.LogArtistCardStored(artistId, card.Id());
    }

    private void LogFailure(string artistId, IScryfallCard card, OpResponse<ScryfallArtistCardExtEntity> response)
    {
        if (response.IsSuccessful()) return;

        _logger.LogArtistCardStoreFailed(artistId, card.Id(), response.StatusCode);
    }
}

internal static partial class ArtistCardsProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Successfully stored card {CardId} for artist {ArtistId} in ArtistCards")]
    public static partial void LogArtistCardStored(this ILogger logger, string artistId, string cardId);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to store card {CardId} for artist {ArtistId} in ArtistCards. Status: {Status}")]
    public static partial void LogArtistCardStoreFailed(this ILogger logger, string artistId, string cardId, HttpStatusCode status);
}
