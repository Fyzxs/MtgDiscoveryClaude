using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SealedProducts;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.SealedProducts.Apis;

/// <summary>
/// Specialized adapter interface for sealed products query operations.
///
/// This interface represents the query-specific adapter functionality,
/// separate from the main ISealedProductsAdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   ISealedProductsAdapterService : ISealedProductsQueryAdapter
///
/// Design Decision: Public specialized interface
/// While concrete implementations are internal, the specialized interfaces are public
/// to allow the main service interface to inherit from them and provide a unified API.
///
/// Entity Mapping Approach:
/// - Input: Uses XfrEntity parameters following the layered architecture pattern
/// - Output: Returns ExtEntity types (Aggregator handles ExtToOuf mapping)
/// </summary>
public interface ISealedProductsQueryAdapter
{
    /// <summary>
    /// Retrieves all sealed products for a specific set by set code.
    /// </summary>
    /// <param name="query">The query entity containing the set code</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Collection of sealed product external entities for the specified set</returns>
    Task<IOperationResponse<IEnumerable<SealedProductExtEntity>>> GetBySetCodeAsync(
        ISealedProductsBySetCodeXfrEntity query,
        CancellationToken cancellationToken);
}
