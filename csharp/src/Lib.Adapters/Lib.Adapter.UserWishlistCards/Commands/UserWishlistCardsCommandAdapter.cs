using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserWishlistCards.Apis;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserWishlistCards.Commands;

internal sealed class UserWishlistCardsCommandAdapter : IUserWishlistCardsCommandAdapter
{
    private readonly IAddUserWishlistCardAdapter _addUserWishlistCardAdapter;
    private readonly IRemoveUserWishlistCardAdapter _removeUserWishlistCardAdapter;

    public UserWishlistCardsCommandAdapter(ILogger logger) : this(
        new AddUserWishlistCardAdapter(logger),
        new RemoveUserWishlistCardAdapter(logger))
    { }

    private UserWishlistCardsCommandAdapter(
        IAddUserWishlistCardAdapter addUserWishlistCardAdapter,
        IRemoveUserWishlistCardAdapter removeUserWishlistCardAdapter)
    {
        _addUserWishlistCardAdapter = addUserWishlistCardAdapter;
        _removeUserWishlistCardAdapter = removeUserWishlistCardAdapter;
    }

    public async Task<IOperationResponse<UserWishlistCardExtEntity>> AddUserWishlistCardAsync(IAddUserWishlistCardXfrEntity addUserWishlistCard) => await _addUserWishlistCardAdapter.Execute(addUserWishlistCard).ConfigureAwait(false);

    public async Task<IOperationResponse<UserWishlistCardExtEntity>> RemoveUserWishlistCardAsync(IRemoveUserWishlistCardXfrEntity removeUserWishlistCard) => await _removeUserWishlistCardAdapter.Execute(removeUserWishlistCard).ConfigureAwait(false);
}
