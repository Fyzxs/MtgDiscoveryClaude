using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Sets.Apis;
using Lib.Aggregator.Sets.Exceptions;
using Lib.Aggregator.Sets.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Queries;

internal sealed class AllSetsAggregatorService : IAllSetsAggregatorService
{
    private readonly ISetAdapterService _setAdapterService;
    private readonly ICollectionSetItemExtToItrMapper _setItemMapper;
    private readonly ICollectionSetItemItrToOufMapper _setItemItrToOufMapper;
    private readonly IAllSetsItrToXfrMapper _allSetsMapper;

    public AllSetsAggregatorService(ILogger logger) : this(
        new SetAdapterService(logger),
        new CollectionSetItemExtToItrMapper(),
        new CollectionSetItemItrToOufMapper(),
        new AllSetsItrToXfrMapper())
    { }

    private AllSetsAggregatorService(
        ISetAdapterService setAdapterService,
        ICollectionSetItemExtToItrMapper setItemMapper,
        ICollectionSetItemItrToOufMapper setItemItrToOufMapper,
        IAllSetsItrToXfrMapper allSetsMapper)
    {
        _setAdapterService = setAdapterService;
        _setItemMapper = setItemMapper;
        _setItemItrToOufMapper = setItemItrToOufMapper;
        _allSetsMapper = allSetsMapper;
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> Execute(IAllSetsItrEntity input)
    {
        Lib.Shared.DataModels.Entities.Xfrs.IAllSetsXfrEntity allSetsXfr = await _allSetsMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _setAdapterService.AllSetsAsync(allSetsXfr).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionOufEntity>(new SetsAggregatorOperationException("Failed to retrieve all sets", response.OuterException));
        }

        IEnumerable<ISetItemItrEntity> mappedSets = await _setItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ISetItemCollectionOufEntity oufEntity = await _setItemItrToOufMapper.Map(mappedSets).ConfigureAwait(false);
        return new SuccessOperationResponse<ISetItemCollectionOufEntity>(oufEntity);
    }
}
