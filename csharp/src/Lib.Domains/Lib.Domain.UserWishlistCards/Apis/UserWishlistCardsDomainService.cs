using System.Collections.Generic;
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
    private readonly IUserWishlistCardsQueryDomainService _queryOperations;
    private readonly IUserWishlistCardsCommandDomainService _commandOperations;

    public UserWishlistCardsDomainService(ILogger logger) : this(new UserWishlistCardsQueryDomainService(logger), new UserWishlistCardsCommandDomainService(logger))
    { }

    private UserWishlistCardsDomainService(IUserWishlistCardsQueryDomainService queryOperations, IUserWishlistCardsCommandDomainService commandOperations) => (_queryOperations, _commandOperations) = (queryOperations, commandOperations);

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> AddUserWishlistCardAsync(IUserWishlistCardItrEntity userWishlistCard) => await _commandOperations.AddUserWishlistCardAsync(userWishlistCard);

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> RemoveUserWishlistCardAsync(IUserWishlistCardItrEntity userWishlistCard) => await _commandOperations.RemoveUserWishlistCardAsync(userWishlistCard);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> GetUserWishlistCardsAsync(IUserWishlistCardsQueryItrEntity query) => await _queryOperations.GetUserWishlistCardsAsync(query);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsBySetAsync(IUserWishlistCardsSetItrEntity userWishlistCardsSet) => await _queryOperations.UserWishlistCardsBySetAsync(userWishlistCardsSet);

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsItrEntity userWishlistCards) => await _queryOperations.UserWishlistCardsByIdsAsync(userWishlistCards);
}
