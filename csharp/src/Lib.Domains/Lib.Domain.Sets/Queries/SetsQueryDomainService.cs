using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Sets.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Sets.Queries;

internal sealed class SetsQueryDomainService : ISetsQueryDomainService
{
    private readonly ISetsDomain _setsDomain;
    private readonly ISetsByCodeDomain _setsByCodeDomain;
    private readonly IAllSetsDomain _allSetsDomain;

    public SetsQueryDomainService(ILogger logger) : this(
        new SetsDomain(logger),
        new SetsByCodeDomain(logger),
        new AllSetsDomain(logger))
    { }

    private SetsQueryDomainService(
        ISetsDomain setsDomain,
        ISetsByCodeDomain setsByCodeDomain,
        IAllSetsDomain allSetsDomain)
    {
        _setsDomain = setsDomain;
        _setsByCodeDomain = setsByCodeDomain;
        _allSetsDomain = allSetsDomain;
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsAsync(
        ISetIdsItrEntity setIds,
        CancellationToken cancellationToken)
        => await _setsDomain.Execute(setIds, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(
        ISetCodesItrEntity setCodes,
        CancellationToken cancellationToken)
        => await _setsByCodeDomain.Execute(setCodes, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync(
        IAllSetsItrEntity allSets,
        CancellationToken cancellationToken)
        => await _allSetsDomain.Execute(allSets, cancellationToken).ConfigureAwait(false);
}
