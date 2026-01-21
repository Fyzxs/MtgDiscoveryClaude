using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.MtgDiscovery.Entry.Queries.Sets;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class SetEntryService : ISetEntryService
{
    private readonly ISetsByIdsEntryService _setsByIds;
    private readonly ISetsByCodeEntryService _setsByCode;
    private readonly IAllSetsEntryService _allSets;

    public SetEntryService(ILogger logger) : this(
        new SetsByIdsEntryService(logger),
        new SetsByCodeEntryService(logger),
        new AllSetsEntryService(logger))
    { }

    private SetEntryService(
        ISetsByIdsEntryService setsByIds,
        ISetsByCodeEntryService setsByCode,
        IAllSetsEntryService allSets)
    {
        _setsByIds = setsByIds;
        _setsByCode = setsByCode;
        _allSets = allSets;
    }

    public async Task<IOperationResponse<List<ScryfallSetOutEntity>>> SetsByIdsAsync(ISetIdsArgEntity args) => await _setsByIds.Execute(args).ConfigureAwait(false);

    public async Task<IOperationResponse<List<ScryfallSetOutEntity>>> SetsByCodeAsync(ISetCodesArgEntity args) => await _setsByCode.Execute(args).ConfigureAwait(false);

    public async Task<IOperationResponse<List<ScryfallSetOutEntity>>> AllSetsAsync() => await _allSets.Execute(new NoArgsEntity()).ConfigureAwait(false);
}
