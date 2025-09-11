using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Sets.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Sets.Apis;

/// <summary>
/// Main set adapter service implementation following the passthrough pattern.
/// 
/// This service coordinates all set-related adapter operations by delegating
/// to specialized adapters. Currently delegates to ISetQueryAdapter for all
/// operations, but provides the architectural structure for future expansion.
/// 
/// Future Expansion Examples:
///   - ISetCacheAdapter for caching layer
///   - ISetFallbackAdapter for redundancy
///   - ISetMetricsAdapter for telemetry
/// 
/// Pattern Consistency:
/// Matches EntryService, DomainService, and AggregatorService patterns
/// to maintain predictable architecture across all layers.
/// </summary>
public sealed class SetAdapterService : ISetAdapterService
{
    private readonly ISetQueryAdapter _setQueryAdapter;

    public SetAdapterService(ILogger logger) : this(new SetCosmosQueryAdapter(logger))
    { }

    private SetAdapterService(ISetQueryAdapter setQueryAdapter)
    {
        _setQueryAdapter = setQueryAdapter;
    }

    public async Task<IOperationResponse<IEnumerable<ISetItemItrEntity>>> GetSetsByIdsAsync(ISetIdsItrEntity setIds)
    {
        return await _setQueryAdapter.GetSetsByIdsAsync(setIds).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<ISetItemItrEntity>>> GetSetsByCodesAsync(ISetCodesItrEntity setCodes)
    {
        return await _setQueryAdapter.GetSetsByCodesAsync(setCodes).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<ISetItemItrEntity>>> GetAllSetsAsync()
    {
        return await _setQueryAdapter.GetAllSetsAsync().ConfigureAwait(false);
    }
}