using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Artists.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Artists.Queries;

/// <summary>
/// Single-method service for artist search operation.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class ArtistSearchDomain : IArtistSearchDomain
{
    private readonly IArtistAggregatorService _artistAggregatorService;

    public ArtistSearchDomain(ILogger logger) : this(new ArtistAggregatorService(logger))
    { }

    private ArtistSearchDomain(IArtistAggregatorService artistAggregatorService) => _artistAggregatorService = artistAggregatorService;

    public async Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> Execute(
        IArtistSearchTermItrEntity input,
        CancellationToken cancellationToken) => await _artistAggregatorService.ArtistSearchAsync(input, cancellationToken).ConfigureAwait(false);
}
