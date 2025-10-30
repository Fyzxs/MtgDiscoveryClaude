using System.Threading.Tasks;
using Lib.Domain.Sets.Queries;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Sets.Apis;

public sealed class SetDomainService : ISetDomainService
{
    private readonly ISetsQueryDomainService _setDomainOperations;

    public SetDomainService(ILogger logger) : this(new SetsQueryDomainService(logger))
    { }

    private SetDomainService(ISetsQueryDomainService setDomainOperations) => _setDomainOperations = setDomainOperations;

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsAsync(ISetIdsItrEntity setIds) => await _setDomainOperations.SetsAsync(setIds).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(ISetCodesItrEntity setCodes) => await _setDomainOperations.SetsByCodeAsync(setCodes).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync(IAllSetsItrEntity allSets) => await _setDomainOperations.AllSetsAsync(allSets).ConfigureAwait(false);
}
