using System.Threading.Tasks;
using Lib.Aggregator.Artists.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Artists.Queries;

/// <summary>
/// Single-method service for retrieving cards by artist name.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class CardsByArtistNameDomainService : ICardsByArtistNameDomainService
{
    private readonly IArtistAggregatorService _artistAggregatorService;

    public CardsByArtistNameDomainService(IArtistAggregatorService artistAggregatorService) => _artistAggregatorService = artistAggregatorService;

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(IArtistNameItrEntity input) => await _artistAggregatorService.CardsByArtistNameAsync(input).ConfigureAwait(false);
}
