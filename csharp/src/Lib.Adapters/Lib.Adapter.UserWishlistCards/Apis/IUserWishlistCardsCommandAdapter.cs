using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserWishlistCards.Apis;

/// <summary>
/// Specialized adapter interface for user wishlist cards command operations.
///
/// This interface represents the command-specific adapter functionality,
/// separate from the main IUserWishlistCardsAdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   IUserWishlistCardsAdapterService : IUserWishlistCardsQueryAdapter, IUserWishlistCardsCommandAdapter
///
/// Design Decision: Public specialized interface
/// While concrete implementations are internal, the specialized interfaces are public
/// to allow the main service interface to inherit from them and provide a unified API.
///
/// Entity Mapping Approach:
/// - Input: Uses XfrEntity parameters following the layered architecture pattern
/// - Output: Returns ExtEntity types from storage systems
/// - Aggregator layer handles mapping from ItrEntity to XfrEntity and ExtEntity to OufEntity
/// </summary>
public interface IUserWishlistCardsCommandAdapter
{
    /// <summary>
    /// Adds a card to the user's wishlist.
    /// </summary>
    /// <param name="addUserWishlistCard">The entity containing the card to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IOperationResponse<UserWishlistCardExtEntity>> AddUserWishlistCardAsync(IAddUserWishlistCardXfrEntity addUserWishlistCard, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a card from the user's wishlist.
    /// </summary>
    /// <param name="removeUserWishlistCard">The entity containing the card to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IOperationResponse<UserWishlistCardExtEntity>> RemoveUserWishlistCardAsync(IRemoveUserWishlistCardXfrEntity removeUserWishlistCard, CancellationToken cancellationToken);
}
