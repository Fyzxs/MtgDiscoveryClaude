using System.Threading.Tasks;
using Lib.Domain.Sets.Apis;
using Lib.MtgDiscovery.Data.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Data.Queries;

internal sealed class SetDataService : ISetDataService
{
    private readonly ISetDomainService _setDomainService;

    public SetDataService(ILogger logger) : this(new SetDomainService(logger))
    {
    }

    private SetDataService(ISetDomainService setDomainService)
    {
        _setDomainService = setDomainService;
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsAsync(ISetIdsItrEntity setIds)
    {
        return await _setDomainService.SetsAsync(setIds).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesItrEntity setCodes)
    {
        return await _setDomainService.SetsByCodeAsync(setCodes).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> AllSetsAsync()
    {
        return await _setDomainService.AllSetsAsync().ConfigureAwait(false);
    }
}