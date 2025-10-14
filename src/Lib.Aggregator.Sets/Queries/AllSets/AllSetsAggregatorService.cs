using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Sets.Apis;
using Lib.Aggregator.Sets.Exceptions;
using Lib.Aggregator.Sets.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Queries.AllSets;

internal sealed class AllSetsAggregatorService : IAllSetsAggregatorService
{
    private readonly ISetAdapterService _setAdapterService;
    private readonly ICollectionSetItemExtToItrMapper _setItemMapper;
    private readonly ICollectionSetItemItrToOufMapper _setItemItrToOufMapper;

    public AllSetsAggregatorService(ILogger logger) : this(
        new SetAdapterService(logger),
        new CollectionSetItemExtToItrMapper(),
        new CollectionSetItemItrToOufMapper())
    { }

    private AllSetsAggregatorService(
        ISetAdapterService setAdapterService,
        ICollectionSetItemExtToItrMapper setItemMapper,
        ICollectionSetItemItrToOufMapper setItemItrToOufMapper)
    {
        _setAdapterService = setAdapterService;
        _setItemMapper = setItemMapper;
        _setItemItrToOufMapper = setItemItrToOufMapper;
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync()
    {
        IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _setAdapterService.AllSetsAsync().ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionOufEntity>(new AggregatorOperationException(System.Net.HttpStatusCode.InternalServerError, "Failed to retrieve all sets", response.OuterException));
        }

        IEnumerable<ISetItemItrEntity> mappedSets = await _setItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ISetItemCollectionOufEntity oufEntity = await _setItemItrToOufMapper.Map(mappedSets).ConfigureAwait(false);
        return new SuccessOperationResponse<ISetItemCollectionOufEntity>(oufEntity);
    }
}
