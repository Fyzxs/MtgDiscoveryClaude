using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Sets.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.MtgDiscovery.Entry.Queries.Enrichments;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Sets;

internal sealed class AllSetsEntryService : IAllSetsEntryService
{
    private readonly IAllSetsArgToItrMapper _allSetsMapper;
    private readonly ISetDomainService _setDomainService;
    private readonly ICollectionSetItemOufToOutMapper _setItemOufToOutMapper;
    private readonly IUserSetEnrichment _userSetEnrichment;

    public AllSetsEntryService(ILogger logger) : this(
        new AllSetsArgToItrMapper(),
        new SetDomainService(logger),
        new CollectionSetItemOufToOutMapper(),
        new UserSetEnrichment(logger))
    { }

    private AllSetsEntryService(
        IAllSetsArgToItrMapper allSetsMapper,
        ISetDomainService setDomainService,
        ICollectionSetItemOufToOutMapper setItemOufToOutMapper,
        IUserSetEnrichment userSetEnrichment)
    {
        _allSetsMapper = allSetsMapper;
        _setDomainService = setDomainService;
        _setItemOufToOutMapper = setItemOufToOutMapper;
        _userSetEnrichment = userSetEnrichment;
    }

    public async Task<IOperationResponse<List<SetItemOutEntity>>> Execute(IAllSetsArgEntity input)
    {
        IAllSetsItrEntity allSetsItr = await _allSetsMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<ISetItemCollectionOufEntity> opResponse = await _setDomainService.AllSetsAsync(allSetsItr).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<SetItemOutEntity>>(opResponse.OuterException);

        List<SetItemOutEntity> outEntities = await _setItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

        // Enrich with user set card information if userId is present
        if (input.HasUserId)
        {
            await _userSetEnrichment.Enrich(outEntities, input).ConfigureAwait(false);
        }

        return new SuccessOperationResponse<List<SetItemOutEntity>>(outEntities);
    }
}
