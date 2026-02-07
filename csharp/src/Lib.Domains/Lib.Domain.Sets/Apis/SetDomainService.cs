using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Sets.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Sets.Apis;

public sealed class SetDomainService : ISetDomainService
{
    private readonly ISetsQueryDomainService _setDomainOperations;

    public SetDomainService(ILogger logger) : this(new SetsQueryDomainService(logger))
    { }

    private SetDomainService(ISetsQueryDomainService setDomainOperations) => _setDomainOperations = setDomainOperations;

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsAsync(
        ISetIdsItrEntity setIds,
        CancellationToken cancellationToken)
        => await _setDomainOperations.SetsAsync(setIds, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(
        ISetCodesItrEntity setCodes,
        CancellationToken cancellationToken)
        => await _setDomainOperations.SetsByCodeAsync(setCodes, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync(
        IAllSetsItrEntity allSets,
        CancellationToken cancellationToken)
        => await _setDomainOperations.AllSetsAsync(allSets, cancellationToken).ConfigureAwait(false);
}
