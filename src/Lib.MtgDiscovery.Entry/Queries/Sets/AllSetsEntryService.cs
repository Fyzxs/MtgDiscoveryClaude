using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Sets.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Sets;

internal sealed class AllSetsEntryService : IAllSetsEntryService
{
    private readonly INoArgsArgToItrMapper _noArgsMapper;
    private readonly ISetDomainService _setDomainService;
    private readonly ICollectionSetItemOufToOutMapper _setItemOufToOutMapper;

    public AllSetsEntryService(ILogger logger) : this(
        new NoArgsArgToItrMapper(),
        new SetDomainService(logger),
        new CollectionSetItemOufToOutMapper())
    { }

    private AllSetsEntryService(
        INoArgsArgToItrMapper noArgsMapper,
        ISetDomainService setDomainService,
        ICollectionSetItemOufToOutMapper setItemOufToOutMapper)
    {
        _noArgsMapper = noArgsMapper;
        _setDomainService = setDomainService;
        _setItemOufToOutMapper = setItemOufToOutMapper;
    }

    public async Task<IOperationResponse<List<ScryfallSetOutEntity>>> Execute(INoArgsArgEntity input)
    {
        INoArgsItrEntity noArgsItr = await _noArgsMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<ISetItemCollectionOufEntity> opResponse = await _setDomainService.AllSetsAsync(noArgsItr).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<ScryfallSetOutEntity>>(opResponse.OuterException);

        List<ScryfallSetOutEntity> outEntities = await _setItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<ScryfallSetOutEntity>>(outEntities);
    }
}
