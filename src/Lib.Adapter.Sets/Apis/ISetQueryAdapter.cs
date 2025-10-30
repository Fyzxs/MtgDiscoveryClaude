using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Shared.DataModels.Entities.Xfrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Sets.Apis;

/// <summary>
/// Specialized adapter interface for set query operations.
///
/// This interface represents the query-specific adapter functionality,
/// separate from the main ISetAdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   ISetAdapterService : ISetQueryAdapter, ISetCacheAdapter, ISetMetricsAdapter
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
public interface ISetQueryAdapter
{
    Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> SetsByIdsAsync(ISetIdsXfrEntity setIds);
    Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> SetsByCodesAsync(ISetCodesXfrEntity setCodes);
    Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> AllSetsAsync(IAllSetsXfrEntity allSets);
}
