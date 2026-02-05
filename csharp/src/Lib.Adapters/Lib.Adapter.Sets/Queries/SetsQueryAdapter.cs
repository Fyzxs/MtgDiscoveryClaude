using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetItems;
using Lib.Adapter.Sets.Apis;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Shared.DataModels.Entities.Xfrs.Sets;
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

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> SetsByIdsAsync(
        ISetIdsXfrEntity setIds,
        CancellationToken cancellationToken)
        => await _setsByIdsAdapter.Execute(setIds, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> SetsByCodesAsync(
        ISetCodesXfrEntity setCodes,
        CancellationToken cancellationToken)
        => await _setsByCodesAdapter.Execute(setCodes, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> AllSetsAsync(
        IAllSetsXfrEntity allSets,
        CancellationToken cancellationToken)
        => await _allSetsAdapter.Execute(allSets, cancellationToken).ConfigureAwait(false);
}
