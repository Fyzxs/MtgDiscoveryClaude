using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSetCards.Apis;

/// <summary>
/// Specialized adapter interface for user set card query operations.
///
/// This interface represents the query-specific adapter functionality,
/// separate from the main IUserSetCardsAdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   IUserSetCardsAdapterService : IUserSetCardQueryAdapter, IUserSetCardCommandAdapter
///
/// Design Decision: Public specialized interface
/// While concrete implementations are internal, the specialized interfaces are public
/// to allow the main service interface to inherit from them and provide a unified API.
///
/// Entity Mapping Approach:
/// - Input: Uses XfrEntity parameters following the layered architecture pattern
/// - Output: Returns ExtEntity types from storage systems
/// - Aggregator layer handles mapping from ItrEntity to XfrEntity and ExtEntity to ItrEntity
/// Primitive extraction happens in the concrete implementation when interfacing with external systems.
/// </summary>
public interface IUserSetCardsQueryAdapter
{
    /// <summary>
    /// Gets user set card data from storage.
    ///
    /// Storage-only operation that retrieves existing data from Cosmos DB.
    /// Returns failure if the record does not exist (does NOT create defaults).
    /// </summary>
    /// <param name="readParams">Parameters for getting user set card (userId, setId)</param>
    /// <returns>User set card ExtEntity wrapped in operation response, or failure if not found</returns>
    Task<IOperationResponse<UserSetCardExtEntity>> GetUserSetCardAsync(IUserSetCardGetXfrEntity readParams);

    /// <summary>
    /// Gets all user set card data for a given user from storage.
    ///
    /// Query operation that retrieves all set card records for the specified user from Cosmos DB.
    /// Returns empty collection if no records exist (does NOT fail on empty results).
    /// </summary>
    /// <param name="queryParams">Parameters for querying user set cards (userId)</param>
    /// <returns>Collection of user set card ExtEntities wrapped in operation response</returns>
    Task<IOperationResponse<IEnumerable<UserSetCardExtEntity>>> GetAllUserSetCardsAsync(IAllUserSetCardsXfrEntity queryParams);
}
