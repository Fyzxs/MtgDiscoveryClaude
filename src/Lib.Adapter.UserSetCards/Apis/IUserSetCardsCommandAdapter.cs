using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSetCards.Apis;

/// <summary>
/// Specialized adapter interface for user set cards command operations.
///
/// This interface provides command operations (read-modify-write) for the UserSetCards collection
/// which tracks aggregate collection data per set per user.
///
/// IMPORTANT: This adapter handles ONLY storage operations (read/upsert).
/// No business logic, aggregation calculations, or group management should be performed here.
/// Those responsibilities belong to the UserSetCardsAggregatorService.
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
    /// Upserts user set card data to storage.
    ///
    /// Storage-only operation that writes data to Cosmos DB.
    /// Creates new record or updates existing based on userId/setId combination.
    /// </summary>
    /// <param name="entity">User set card data to persist</param>
    /// <returns>Persisted user set card ExtEntity wrapped in operation response</returns>
    Task<IOperationResponse<UserSetCardExtEntity>> UpsertUserSetCardAsync(IUserSetCardUpsertXfrEntity entity);
}
