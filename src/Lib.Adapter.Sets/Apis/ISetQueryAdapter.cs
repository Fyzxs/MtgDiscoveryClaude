using System.Collections.Generic;
using System.Threading.Tasks;
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
/// - Output: Returns ITR entities for consistency with main service interface
/// - Internal mapping: Adapter implementations map from storage entities to ITR entities
/// Primitive extraction happens in the concrete implementation when interfacing with external systems.
/// </summary>
public interface ISetQueryAdapter
{
    Task<IOperationResponse<IEnumerable<ISetItemItrEntity>>> GetSetsByIdsAsync(ISetIdsItrEntity setIds);
    Task<IOperationResponse<IEnumerable<ISetItemItrEntity>>> GetSetsByCodesAsync(ISetCodesItrEntity setCodes);
    Task<IOperationResponse<IEnumerable<ISetItemItrEntity>>> GetAllSetsAsync();
}