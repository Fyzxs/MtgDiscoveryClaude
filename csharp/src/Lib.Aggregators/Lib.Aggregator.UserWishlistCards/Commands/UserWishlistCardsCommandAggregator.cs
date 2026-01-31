using System.Threading.Tasks;
using Lib.Aggregator.UserWishlistCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserWishlistCards.Commands;

internal sealed class UserWishlistCardsCommandAggregator : IUserWishlistCardsCommandAggregatorService
{
    private readonly IAddUserWishlistCardAggregator _addUserWishlistCardOperations;
    private readonly IRemoveUserWishlistCardAggregator _removeUserWishlistCardOperations;

    public UserWishlistCardsCommandAggregator(ILogger logger) : this(
        new AddUserWishlistCardAggregator(logger),
        new RemoveUserWishlistCardAggregator(logger))
    { }

    private UserWishlistCardsCommandAggregator(
        IAddUserWishlistCardAggregator addUserWishlistCardOperations,
        IRemoveUserWishlistCardAggregator removeUserWishlistCardOperations)
    {
        _addUserWishlistCardOperations = addUserWishlistCardOperations;
        _removeUserWishlistCardOperations = removeUserWishlistCardOperations;
    }

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> AddUserWishlistCardAsync(IUserWishlistCardItrEntity wishlistCard) => await _addUserWishlistCardOperations.Execute(wishlistCard);

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> RemoveUserWishlistCardAsync(IUserWishlistCardItrEntity wishlistCard) => await _removeUserWishlistCardOperations.Execute(wishlistCard);
}
