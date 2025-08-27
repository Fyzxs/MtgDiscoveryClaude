using System.Threading.Tasks;
using Lib.Domain.Sets.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Sets.Apis;

public sealed class SetDomainService : ISetDomainService
{
    private readonly ISetDomainService _setDomainService;

    public SetDomainService(ILogger logger) : this(new QuerySetDomainService(logger) as ISetDomainService)
    {
    }

    private SetDomainService(ISetDomainService setDomainService)
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