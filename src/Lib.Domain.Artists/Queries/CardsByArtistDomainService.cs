using System.Threading.Tasks;
using Lib.Aggregator.Artists.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Artists.Queries;

/// <summary>
/// Single-method service for retrieving cards by artist ID.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class CardsByArtistDomainService : ICardsByArtistDomainService
{
    private readonly IArtistAggregatorService _artistAggregatorService;

    public CardsByArtistDomainService(IArtistAggregatorService artistAggregatorService) => _artistAggregatorService = artistAggregatorService;

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(IArtistIdItrEntity input) => await _artistAggregatorService.CardsByArtistAsync(input).ConfigureAwait(false);
}
