using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserWishlistCards.Commands;
using Lib.Domain.UserWishlistCards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserWishlistCards.Apis;

public sealed class UserWishlistCardsDomainService : IUserWishlistCardsDomainService
{
    private readonly IUserWishlistCardsQueryDomainService _queryService;
    private readonly IUserWishlistCardsCommandDomainService _commandService;

    public UserWishlistCardsDomainService(ILogger logger) : this(new UserWishlistCardsQueryDomainService(logger), new UserWishlistCardsCommandDomainService(logger))
    { }

    private UserWishlistCardsDomainService(IUserWishlistCardsQueryDomainService queryService, IUserWishlistCardsCommandDomainService commandService) => (_queryService, _commandService) = (queryService, commandService);

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> AddUserWishlistCardAsync(IUserWishlistCardItrEntity userWishlistCard, CancellationToken cancellationToken) => await _commandService.AddUserWishlistCardAsync(userWishlistCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> RemoveUserWishlistCardAsync(IUserWishlistCardItrEntity userWishlistCard, CancellationToken cancellationToken) => await _commandService.RemoveUserWishlistCardAsync(userWishlistCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> GetUserWishlistCardsAsync(IUserWishlistCardsQueryItrEntity query, CancellationToken cancellationToken) => await _queryService.GetUserWishlistCardsAsync(query, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsBySetAsync(IUserWishlistCardsSetItrEntity userWishlistCardsSet, CancellationToken cancellationToken) => await _queryService.UserWishlistCardsBySetAsync(userWishlistCardsSet, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsItrEntity userWishlistCards, CancellationToken cancellationToken) => await _queryService.UserWishlistCardsByIdsAsync(userWishlistCards, cancellationToken).ConfigureAwait(false);
}
