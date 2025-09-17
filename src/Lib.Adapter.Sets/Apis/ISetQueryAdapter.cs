using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities;
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
/// - Input: Preserves ItrEntity parameters following MicroObjects principles
/// - Output: Returns ExtEntity types from storage systems
/// - Aggregator layer handles mapping from ExtEntity to ItrEntity
/// Primitive extraction happens in the concrete implementation when interfacing with external systems.
/// </summary>
public interface ISetQueryAdapter
{
    Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> GetSetsByIdsAsync(ISetIdsItrEntity setIds);
    Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> GetSetsByCodesAsync(ISetCodesItrEntity setCodes);
    Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> GetAllSetsAsync();
}
