using System.Threading.Tasks;
using Lib.Aggregator.Artists.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Apis;

public sealed class ArtistAggregatorService : IArtistAggregatorService
{
    private readonly IArtistAggregatorService _artistAggregatorOperations;

    public ArtistAggregatorService(ILogger logger) : this(new QueryArtistAggregatorService(logger))
    { }

    private ArtistAggregatorService(IArtistAggregatorService artistAggregatorOperations) => _artistAggregatorOperations = artistAggregatorOperations;

    public Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm) => _artistAggregatorOperations.ArtistSearchAsync(searchTerm);

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId) => _artistAggregatorOperations.CardsByArtistAsync(artistId);

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName) => _artistAggregatorOperations.CardsByArtistNameAsync(artistName);
}
