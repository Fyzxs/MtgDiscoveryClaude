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
    private readonly ISetsDomainService _setsService;
    private readonly ISetsByCodeDomainService _setsByCodeService;
    private readonly IAllSetsDomainService _allSetsService;

    public SetsQueryDomainService(ILogger logger) : this(
        new SetsDomainService(logger),
        new SetsByCodeDomainService(logger),
        new AllSetsDomainService(logger))
    { }

    private SetsQueryDomainService(
        ISetsDomainService setsService,
        ISetsByCodeDomainService setsByCodeService,
        IAllSetsDomainService allSetsService)
    {
        _setsService = setsService;
        _setsByCodeService = setsByCodeService;
        _allSetsService = allSetsService;
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsAsync(
        ISetIdsItrEntity setIds,
        CancellationToken cancellationToken)
        => await _setsService.Execute(setIds, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(
        ISetCodesItrEntity setCodes,
        CancellationToken cancellationToken)
        => await _setsByCodeService.Execute(setCodes, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync(
        IAllSetsItrEntity allSets,
        CancellationToken cancellationToken)
        => await _allSetsService.Execute(allSets, cancellationToken).ConfigureAwait(false);
}
