using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
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

    public async Task<IOperationResponse<UserWishlistCardExtEntity>> AddUserWishlistCardAsync(IAddUserWishlistCardXfrEntity addUserWishlistCard, CancellationToken cancellationToken) => await _addUserWishlistCardAdapter.Execute(addUserWishlistCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<UserWishlistCardExtEntity>> RemoveUserWishlistCardAsync(IRemoveUserWishlistCardXfrEntity removeUserWishlistCard, CancellationToken cancellationToken) => await _removeUserWishlistCardAdapter.Execute(removeUserWishlistCard, cancellationToken).ConfigureAwait(false);
}
