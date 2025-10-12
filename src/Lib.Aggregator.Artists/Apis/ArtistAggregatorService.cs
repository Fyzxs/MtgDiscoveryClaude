using System.Threading.Tasks;
using Lib.Aggregator.Artists.Queries;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Apis;

public sealed class ArtistAggregatorService : IArtistAggregatorService
{
    private readonly IArtistAggregatorService _artistAggregatorOperations;

    public ArtistAggregatorService(ILogger logger) : this(new ArtistsQueryAggregator(logger))
    { }

    private ArtistAggregatorService(IArtistAggregatorService artistAggregatorOperations) => _artistAggregatorOperations = artistAggregatorOperations;

    public Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm) => _artistAggregatorOperations.ArtistSearchAsync(searchTerm);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId) => _artistAggregatorOperations.CardsByArtistAsync(artistId);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName) => _artistAggregatorOperations.CardsByArtistNameAsync(artistName);
}
