using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Domain.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Queries;

internal sealed class QueryUserCardsDomainService : IUserCardsQueryDomainService
{
    private readonly IUserCardsQueryAggregatorService _userCardsAggregatorService;

    public QueryUserCardsDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private QueryUserCardsDomainService(IUserCardsQueryAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => await _userCardsAggregatorService.UserCardAsync(userCard).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => await _userCardsAggregatorService.UserCardsBySetAsync(userCardsSet).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => await _userCardsAggregatorService.UserCardsByIdsAsync(userCards).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(IUserCardsArtistItrEntity userCardsArtist) => await _userCardsAggregatorService.UserCardsByArtistAsync(userCardsArtist).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName) => await _userCardsAggregatorService.UserCardsByNameAsync(userCardsName).ConfigureAwait(false);
}
