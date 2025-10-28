using System.Threading.Tasks;
using Lib.Aggregator.Artists.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Artists.Queries;

/// <summary>
/// Single-method service for artist search operation.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class ArtistSearchDomainService : IArtistSearchDomainService
{
    private readonly IArtistAggregatorService _artistAggregatorService;

    public ArtistSearchDomainService(IArtistAggregatorService artistAggregatorService) => _artistAggregatorService = artistAggregatorService;

    public async Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> Execute(IArtistSearchTermItrEntity input) => await _artistAggregatorService.ArtistSearchAsync(input).ConfigureAwait(false);
}
