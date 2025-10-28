using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Sets.Apis;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Shared.DataModels.Entities.Xfrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Sets.Queries;

/// <summary>
/// Cosmos DB implementation of the set query adapter.
///
/// This class coordinates all Cosmos DB-specific set query operations
/// by delegating to specialized single-method adapters.
/// The main SetAdapterService delegates to this implementation.
/// </summary>
internal sealed class SetsQueryAdapter : ISetQueryAdapter
{
    private readonly ISetsByIdsAdapter _setsByIdsAdapter;
    private readonly ISetsByCodesAdapter _setsByCodesAdapter;
    private readonly IAllSetsAdapter _allSetsAdapter;

    public SetsQueryAdapter(ILogger logger) : this(
        new SetsByIdsAdapter(logger),
        new SetsByCodesAdapter(logger),
        new AllSetsAdapter(logger))
    { }

    private SetsQueryAdapter(
        ISetsByIdsAdapter setsByIdsAdapter,
        ISetsByCodesAdapter setsByCodesAdapter,
        IAllSetsAdapter allSetsAdapter)
    {
        _setsByIdsAdapter = setsByIdsAdapter;
        _setsByCodesAdapter = setsByCodesAdapter;
        _allSetsAdapter = allSetsAdapter;
    }

    public Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> SetsByIdsAsync(ISetIdsXfrEntity setIds) => _setsByIdsAdapter.Execute(setIds);

    public Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> SetsByCodesAsync(ISetCodesXfrEntity setCodes) => _setsByCodesAdapter.Execute(setCodes);

    public Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> AllSetsAsync(INoArgsXfrEntity noArgs) => _allSetsAdapter.Execute(noArgs);
}
