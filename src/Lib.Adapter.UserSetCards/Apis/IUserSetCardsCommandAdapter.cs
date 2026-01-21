using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSetCards.Apis;

/// <summary>
/// Specialized adapter interface for user set cards command operations.
///
/// This interface provides command operations (read-modify-write) for the UserSetCards collection
/// which tracks aggregate collection data per set per user.
///
/// Entity Mapping Approach:
/// - Input: Uses XfrEntity parameters following the layered architecture pattern
/// - Output: Returns ExtEntity types from storage systems
/// - Aggregator layer handles mapping from ItrEntity to XfrEntity and ExtEntity to ItrEntity
/// Primitive extraction happens in the concrete implementation when interfacing with external systems.
///
/// Note: Read operation supports the aggregator's read-modify-write pattern for aggregation updates.
/// </summary>
public interface IUserSetCardsCommandAdapter
{
    /// <summary>
    /// Adds or removes a card from a user's set collection.
    ///
    /// This method implements the atomic read-modify-write operation:
    /// 1. Retrieves existing record (or creates new if none exists)
    /// 2. Updates totals and card collections based on count (positive=add, negative=remove)
    /// 3. Upserts the modified record
    ///
    /// This logic is intrinsic to maintaining UserSetCard data integrity.
    /// </summary>
    /// <param name="entity">Card modification parameters</param>
    /// <returns>Updated user set card ExtEntity wrapped in operation response</returns>
    Task<IOperationResponse<UserSetCardExtEntity>> AddCardToSetAsync(IAddCardToSetXfrEntity entity);

    /// <summary>
    /// Adds a set group to a user's set collection tracking.
    ///
    /// This method implements the atomic read-modify-write operation:
    /// 1. Retrieves existing record (or creates new if none exists)
    /// 2. Updates the Collecting array with the set group if not already present
    /// 3. Upserts the modified record
    ///
    /// This logic is intrinsic to maintaining UserSetCard collecting status.
    /// </summary>
    /// <param name="entity">Set group modification parameters</param>
    /// <returns>Updated user set card ExtEntity wrapped in operation response</returns>
    Task<IOperationResponse<UserSetCardExtEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardXfrEntity entity);
}
