using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Sets.Apis;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Aggregator.Sets.Exceptions;
using Lib.Aggregator.Sets.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Queries.SetsById;

internal sealed class SetsByIdAggregatorService : ISetsByIdAggregatorService
{
    private readonly ISetAdapterService _setAdapterService;
    private readonly ICollectionSetItemExtToItrMapper _setItemMapper;
    private readonly ISetIdsItrToXfrMapper _setIdsItrToXfrMapper;
    private readonly ICollectionSetItemItrToOufMapper _setItemItrToOufMapper;

    public SetsByIdAggregatorService(ILogger logger) : this(
        new SetAdapterService(logger),
        new CollectionSetItemExtToItrMapper(),
        new SetIdsItrToXfrMapper(),
        new CollectionSetItemItrToOufMapper())
    { }

    private SetsByIdAggregatorService(
        ISetAdapterService setAdapterService,
        ICollectionSetItemExtToItrMapper setItemMapper,
        ISetIdsItrToXfrMapper setIdsItrToXfrMapper,
        ICollectionSetItemItrToOufMapper setItemItrToOufMapper)
    {
        _setAdapterService = setAdapterService;
        _setItemMapper = setItemMapper;
        _setIdsItrToXfrMapper = setIdsItrToXfrMapper;
        _setItemItrToOufMapper = setItemItrToOufMapper;
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> Execute(ISetIdsItrEntity input)
    {
        ISetIdsXfrEntity xfrEntity = await _setIdsItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _setAdapterService.SetsByIdsAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionOufEntity>(new SetsAggregatorOperationException("Failed to retrieve sets by IDs", response.OuterException));
        }

        IEnumerable<ISetItemItrEntity> mappedSets = await _setItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ISetItemCollectionOufEntity oufEntity = await _setItemItrToOufMapper.Map(mappedSets).ConfigureAwait(false);
        return new SuccessOperationResponse<ISetItemCollectionOufEntity>(oufEntity);
    }

}
