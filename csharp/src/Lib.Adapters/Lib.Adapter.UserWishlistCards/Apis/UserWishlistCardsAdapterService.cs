using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Adapter.UserWishlistCards.Commands;
using Lib.Adapter.UserWishlistCards.Queries;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserWishlistCards.Apis;

/// <summary>
/// Composite service that coordinates all user wishlist cards adapter operations.
/// Implements the passthrough pattern, delegating to specialized query and command adapters.
///
/// Architecture: AdapterService → QueryAdapter/CommandAdapter → Single-Operation Adapters
///
/// This service provides a unified API for all user wishlist cards operations while internally
/// organizing the implementation across specialized adapters for queries and commands.
/// </summary>
public sealed class UserWishlistCardsAdapterService : IUserWishlistCardsAdapterService
{
    private readonly IUserWishlistCardsCommandAdapter _userWishlistCardsCommandAdapter;
    private readonly IUserWishlistCardsQueryAdapter _userWishlistCardsQueryAdapter;

    public UserWishlistCardsAdapterService(ILogger logger) : this(
        new UserWishlistCardsCommandAdapter(logger),
        new UserWishlistCardsQueryAdapter(logger))
    { }

    private UserWishlistCardsAdapterService(
        IUserWishlistCardsCommandAdapter userWishlistCardsCommandAdapter,
        IUserWishlistCardsQueryAdapter userWishlistCardsQueryAdapter)
    {
        _userWishlistCardsCommandAdapter = userWishlistCardsCommandAdapter;
        _userWishlistCardsQueryAdapter = userWishlistCardsQueryAdapter;
    }

    public async Task<IOperationResponse<UserWishlistCardExtEntity>> AddUserWishlistCardAsync(IAddUserWishlistCardXfrEntity addUserWishlistCard) => await _userWishlistCardsCommandAdapter.AddUserWishlistCardAsync(addUserWishlistCard).ConfigureAwait(false);

    public async Task<IOperationResponse<UserWishlistCardExtEntity>> RemoveUserWishlistCardAsync(IRemoveUserWishlistCardXfrEntity removeUserWishlistCard) => await _userWishlistCardsCommandAdapter.RemoveUserWishlistCardAsync(removeUserWishlistCard).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserWishlistCardExtEntity>>> UserWishlistCardsByUserAsync(IUserWishlistCardXfrEntity userWishlistCard) => await _userWishlistCardsQueryAdapter.UserWishlistCardsByUserAsync(userWishlistCard).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserWishlistCardExtEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsXfrEntity userWishlistCards) => await _userWishlistCardsQueryAdapter.UserWishlistCardsByIdsAsync(userWishlistCards).ConfigureAwait(false);
}
