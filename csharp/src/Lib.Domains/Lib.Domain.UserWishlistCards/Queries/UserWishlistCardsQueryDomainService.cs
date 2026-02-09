using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserWishlistCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserWishlistCards.Queries;

internal sealed class UserWishlistCardsQueryDomainService : IUserWishlistCardsQueryDomainService
{
    private readonly IGetUserWishlistCardsDomain _getUserWishlistCardsService;
    private readonly IUserWishlistCardsBySetDomain _userWishlistCardsBySetService;
    private readonly IUserWishlistCardsByIdsDomain _userWishlistCardsByIdsService;

    public UserWishlistCardsQueryDomainService(ILogger logger) : this(
        new GetUserWishlistCardsDomain(logger),
        new UserWishlistCardsBySetDomain(logger),
        new UserWishlistCardsByIdsDomain(logger))
    { }

    private UserWishlistCardsQueryDomainService(
        IGetUserWishlistCardsDomain getUserWishlistCardsService,
        IUserWishlistCardsBySetDomain userWishlistCardsBySetService,
        IUserWishlistCardsByIdsDomain userWishlistCardsByIdsService)
    {
        _getUserWishlistCardsService = getUserWishlistCardsService;
        _userWishlistCardsBySetService = userWishlistCardsBySetService;
        _userWishlistCardsByIdsService = userWishlistCardsByIdsService;
    }

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> GetUserWishlistCardsAsync(IUserWishlistCardsQueryItrEntity query, CancellationToken cancellationToken) => await _getUserWishlistCardsService.Execute(query, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsBySetAsync(IUserWishlistCardsSetItrEntity userWishlistCardsSet, CancellationToken cancellationToken) => await _userWishlistCardsBySetService.Execute(userWishlistCardsSet, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsItrEntity userWishlistCards, CancellationToken cancellationToken) => await _userWishlistCardsByIdsService.Execute(userWishlistCards, cancellationToken).ConfigureAwait(false);
}
