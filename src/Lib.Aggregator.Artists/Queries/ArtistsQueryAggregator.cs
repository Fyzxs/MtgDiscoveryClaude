using System.Threading.Tasks;
using Lib.Aggregator.Artists.Apis;
using Lib.Aggregator.Artists.Queries.ArtistSearch;
using Lib.Aggregator.Artists.Queries.CardsByArtist;
using Lib.Aggregator.Artists.Queries.CardsByArtistName;
using Lib.Shared.DataModels.Entities.Itrs;
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

    public Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm) => _artistSearchOperations.Execute(searchTerm);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId) => _cardsByArtistOperations.Execute(artistId);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName) => _cardsByArtistNameOperations.Execute(artistName);
}
