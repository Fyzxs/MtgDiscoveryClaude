using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Apis;

/// <summary>
/// Specialized adapter interface for user card collection query operations.
///
/// This interface represents the query-specific adapter functionality,
/// separate from the main IUserCardsAdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   IUserCardsAdapterService : IUserCardsCommandAdapter, IUserCardsQueryAdapter
///
/// Design Decision: Public specialized interface
/// While concrete implementations are internal, the specialized interfaces are public
/// to allow the main service interface to inherit from them and provide a unified API.
///
/// Entity Mapping Approach:
/// - Input: Accepts primitive parameters for query operations
/// - Output: Returns ITR entities for consistency with main service interface
/// - Internal mapping: Adapter implementations map from storage entities to ITR entities
/// </summary>
public interface IUserCardsQueryAdapter
{
    /// <summary>
    /// Retrieves all user cards for a specific user within a given set.
    /// </summary>
    /// <param name="userCardsSet">The user cards set entity containing userId and setId</param>
    /// <returns>Collection of user card collection information wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet);
}
