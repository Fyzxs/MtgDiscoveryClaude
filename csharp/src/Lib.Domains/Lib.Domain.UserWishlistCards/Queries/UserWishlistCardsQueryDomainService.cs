using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserWishlistCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserWishlistCards.Queries;

internal sealed class UserWishlistCardsQueryDomainService : IUserWishlistCardsQueryDomainService
{
    private readonly IGetUserWishlistCardsDomainService _getUserWishlistCardsService;
    private readonly IUserWishlistCardsBySetDomainService _userWishlistCardsBySetService;
    private readonly IUserWishlistCardsByIdsDomainService _userWishlistCardsByIdsService;

    public UserWishlistCardsQueryDomainService(ILogger logger) : this(
        new GetUserWishlistCardsDomainService(logger),
        new UserWishlistCardsBySetDomainService(logger),
        new UserWishlistCardsByIdsDomainService(logger))
    { }

    private UserWishlistCardsQueryDomainService(
        IGetUserWishlistCardsDomainService getUserWishlistCardsService,
        IUserWishlistCardsBySetDomainService userWishlistCardsBySetService,
        IUserWishlistCardsByIdsDomainService userWishlistCardsByIdsService)
    {
        _getUserWishlistCardsService = getUserWishlistCardsService;
        _userWishlistCardsBySetService = userWishlistCardsBySetService;
        _userWishlistCardsByIdsService = userWishlistCardsByIdsService;
    }

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> GetUserWishlistCardsAsync(IUserWishlistCardsQueryItrEntity query) => await _getUserWishlistCardsService.Execute(query).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsBySetAsync(IUserWishlistCardsSetItrEntity userWishlistCardsSet) => await _userWishlistCardsBySetService.Execute(userWishlistCardsSet).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsItrEntity userWishlistCards) => await _userWishlistCardsByIdsService.Execute(userWishlistCards).ConfigureAwait(false);
}
