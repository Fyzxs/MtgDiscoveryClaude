using System.Threading.Tasks;
using Lib.Aggregator.Sets.Apis;
using Lib.Domain.Sets.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
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

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsAsync(ISetIdsItrEntity setIds)
        => await _setsService.Execute(setIds).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(ISetCodesItrEntity setCodes)
        => await _setsByCodeService.Execute(setCodes).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync(INoArgsItrEntity noArgs)
        => await _allSetsService.Execute(noArgs).ConfigureAwait(false);
}
