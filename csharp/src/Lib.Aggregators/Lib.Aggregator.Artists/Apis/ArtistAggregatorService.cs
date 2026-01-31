using System.Threading.Tasks;
using Lib.Aggregator.Artists.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Apis;

public sealed class ArtistAggregatorService : IArtistAggregatorService
{
    private readonly IArtistsQueryAggregatorService _artistAggregatorOperations;

    public ArtistAggregatorService(ILogger logger) : this(new ArtistsQueryAggregator(logger))
    { }

    private ArtistAggregatorService(IArtistsQueryAggregatorService artistAggregatorOperations) => _artistAggregatorOperations = artistAggregatorOperations;

    public async Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm) => await _artistAggregatorOperations.ArtistSearchAsync(searchTerm);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId) => await _artistAggregatorOperations.CardsByArtistAsync(artistId);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName) => await _artistAggregatorOperations.CardsByArtistNameAsync(artistName);
}
