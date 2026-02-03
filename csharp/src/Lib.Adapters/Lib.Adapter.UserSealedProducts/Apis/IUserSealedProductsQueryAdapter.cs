using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSealedProducts.Apis;

/// <summary>
/// Specialized adapter interface for user sealed products query operations.
///
/// This interface represents the query-specific adapter functionality,
/// separate from the main IUserSealedProductsAdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   IUserSealedProductsAdapterService : IUserSealedProductsQueryAdapter, IUserSealedProductsCommandAdapter
///
/// Design Decision: Public specialized interface
/// While concrete implementations are internal, the specialized interfaces are public
/// to allow the main service interface to inherit from them and provide a unified API.
///
/// Entity Mapping Approach:
/// - Input: Currently uses raw string parameter (to be migrated to XfrEntity)
/// - Output: Returns ExtEntity types from storage systems
/// - Aggregator layer handles mapping from ExtEntity to OufEntity
/// </summary>
public interface IUserSealedProductsQueryAdapter
{
    /// <summary>
    /// Retrieves all sealed products for a specific user by their collection ID.
    /// </summary>
    /// <param name="collectionId">The collection identifier to query sealed products for</param>
    Task<IOperationResponse<IEnumerable<UserSealedProductExtEntity>>> UserSealedProductsByUserIdAsync(string collectionId);
}
