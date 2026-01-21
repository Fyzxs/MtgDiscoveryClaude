using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Domain.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Query domain service for user card operations.
/// Delegates to single-method services following the Execute pattern.
/// </summary>
internal sealed class UserCardsQueryDomainService : IUserCardsQueryDomainService
{
    private readonly IUserCardDomainService _userCardService;
    private readonly IUserCardsBySetDomainService _userCardsBySetService;
    private readonly IUserCardsByIdsDomainService _userCardsByIdsService;
    private readonly IUserCardsByArtistDomainService _userCardsByArtistService;
    private readonly IUserCardsByNameDomainService _userCardsByNameService;

    public UserCardsQueryDomainService(ILogger logger)
        : this(
            new UserCardsAggregatorService(logger))
    {
    }

    private UserCardsQueryDomainService(IUserCardsQueryAggregatorService userCardsAggregatorService)
    {
        _userCardService = new UserCardDomainService(userCardsAggregatorService);
        _userCardsBySetService = new UserCardsBySetDomainService(userCardsAggregatorService);
        _userCardsByIdsService = new UserCardsByIdsDomainService(userCardsAggregatorService);
        _userCardsByArtistService = new UserCardsByArtistDomainService(userCardsAggregatorService);
        _userCardsByNameService = new UserCardsByNameDomainService(userCardsAggregatorService);
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => await _userCardService.Execute(userCard).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => await _userCardsBySetService.Execute(userCardsSet).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => await _userCardsByIdsService.Execute(userCards).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(IUserCardsArtistItrEntity userCardsArtist) => await _userCardsByArtistService.Execute(userCardsArtist).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName) => await _userCardsByNameService.Execute(userCardsName).ConfigureAwait(false);
}
