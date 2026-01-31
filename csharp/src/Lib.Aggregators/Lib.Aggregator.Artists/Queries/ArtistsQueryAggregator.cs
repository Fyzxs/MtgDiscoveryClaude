using System.Threading.Tasks;
using Lib.Aggregator.Artists.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Queries;

internal sealed class ArtistsQueryAggregator : IArtistAggregatorService
{
    private readonly IArtistSearchAggregatorService _artistSearchOperations;
    private readonly ICardsByArtistAggregatorService _cardsByArtistOperations;
    private readonly ICardsByArtistNameAggregatorService _cardsByArtistNameOperations;

    public ArtistsQueryAggregator(ILogger logger) : this(
        new ArtistSearchAggregatorService(logger),
        new CardsByArtistAggregatorService(logger),
        new CardsByArtistNameAggregatorService(logger))
    { }

    private ArtistsQueryAggregator(
        IArtistSearchAggregatorService artistSearchOperations,
        ICardsByArtistAggregatorService cardsByArtistOperations,
        ICardsByArtistNameAggregatorService cardsByArtistNameOperations)
    {
        _artistSearchOperations = artistSearchOperations;
        _cardsByArtistOperations = cardsByArtistOperations;
        _cardsByArtistNameOperations = cardsByArtistNameOperations;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm) => await _artistSearchOperations.Execute(searchTerm);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId) => await _cardsByArtistOperations.Execute(artistId);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName) => await _cardsByArtistNameOperations.Execute(artistName);
}
