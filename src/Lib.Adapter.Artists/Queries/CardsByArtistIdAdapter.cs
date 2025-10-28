using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Artists.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Artists.Queries;

/// <summary>
/// Retrieves cards by artist ID using Cosmos DB query.
/// </summary>
internal sealed class CardsByArtistIdAdapter : ICardsByArtistIdAdapter
{
    private readonly ICosmosInquisition<CardsByArtistIdInquisitionArgs> _cardsByArtistIdInquisition;

    public CardsByArtistIdAdapter(ILogger logger) : this(new CardsByArtistIdInquisition(logger)) { }

    private CardsByArtistIdAdapter(ICosmosInquisition<CardsByArtistIdInquisitionArgs> cardsByArtistIdInquisition) =>
        _cardsByArtistIdInquisition = cardsByArtistIdInquisition;

    public async Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> Execute([NotNull] IArtistIdXfrEntity input)
    {
        // Query all cards for this artist ID using the artist ID as partition key
        CardsByArtistIdInquisitionArgs args = new() { ArtistId = input.ArtistId };

        OpResponse<IEnumerable<ScryfallArtistCardExtEntity>> cardsResponse = await _cardsByArtistIdInquisition
            .QueryAsync<ScryfallArtistCardExtEntity>(args)
            .ConfigureAwait(false);

        if (cardsResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>(
                new ArtistAdapterException($"Failed to retrieve cards for artist '{input.ArtistId}'", cardsResponse.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>(cardsResponse.Value);
    }
}
