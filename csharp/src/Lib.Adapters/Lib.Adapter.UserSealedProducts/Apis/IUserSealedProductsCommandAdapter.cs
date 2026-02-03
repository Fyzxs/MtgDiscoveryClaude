using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSealedProducts.Apis;

/// <summary>
/// Specialized adapter interface for user sealed products command operations.
///
/// This interface represents the command-specific adapter functionality,
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
/// - Input: Uses XfrEntity parameters following the layered architecture pattern
/// - Output: Returns ExtEntity types from storage systems
/// - Aggregator layer handles mapping from ItrEntity to XfrEntity and ExtEntity to OufEntity
/// </summary>
public interface IUserSealedProductsCommandAdapter
{
    /// <summary>
    /// Adds or updates a sealed product in a user's collection.
    /// </summary>
    Task<IOperationResponse<UserSealedProductExtEntity>> AddUserSealedProductAsync(IUserSealedProductXfrEntity input);
}
