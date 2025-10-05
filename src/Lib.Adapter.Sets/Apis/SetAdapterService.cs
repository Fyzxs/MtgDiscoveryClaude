using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Adapter.Sets.Queries;
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

    public SetAdapterService(ILogger logger) : this(new SetsQueryAdapter(logger))
    { }

    private SetAdapterService(ISetQueryAdapter setQueryAdapter) => _setQueryAdapter = setQueryAdapter;

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> GetSetsByIdsAsync(ISetIdsXfrEntity setIds) => await _setQueryAdapter.GetSetsByIdsAsync(setIds).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> GetSetsByCodesAsync(ISetCodesXfrEntity setCodes) => await _setQueryAdapter.GetSetsByCodesAsync(setCodes).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> GetAllSetsAsync() => await _setQueryAdapter.GetAllSetsAsync().ConfigureAwait(false);
}
