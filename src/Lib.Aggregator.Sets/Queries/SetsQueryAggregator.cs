using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Sets.Apis;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Aggregator.Sets.Apis;
using Lib.Aggregator.Sets.Entities;
using Lib.Aggregator.Sets.Exceptions;
using Lib.Aggregator.Sets.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Queries;

internal sealed class SetsQueryAggregator : ISetAggregatorService
{
    private readonly ISetAdapterService _setAdapterService;
    private readonly ICollectionSetItemExtToItrMapper _setItemMapper;
    private readonly ISetIdsItrToXfrMapper _setIdsItrToXfrMapper;
    private readonly ISetCodesItrToXfrMapper _setCodesItrToXfrMapper;
    private readonly ICollectionSetItemItrToOufMapper _setItemItrToOufMapper;

    public SetsQueryAggregator(ILogger logger) : this(
        new SetAdapterService(logger),
        new CollectionSetItemExtToItrMapper(),
        new SetIdsItrToXfrMapper(),
        new SetCodesItrToXfrMapper(),
        new CollectionSetItemItrToOufMapper())
    { }

    private SetsQueryAggregator(
        ISetAdapterService setAdapterService,
        ICollectionSetItemExtToItrMapper setItemMapper,
        ISetIdsItrToXfrMapper setIdsItrToXfrMapper,
        ISetCodesItrToXfrMapper setCodesItrToXfrMapper,
        ICollectionSetItemItrToOufMapper setItemItrToOufMapper)
    {
        _setAdapterService = setAdapterService;
        _setItemMapper = setItemMapper;
        _setIdsItrToXfrMapper = setIdsItrToXfrMapper;
        _setCodesItrToXfrMapper = setCodesItrToXfrMapper;
        _setItemItrToOufMapper = setItemItrToOufMapper;
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsAsync(ISetIdsItrEntity args)
    {
        ISetIdsXfrEntity xfrEntity = await _setIdsItrToXfrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _setAdapterService.SetsByIdsAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionOufEntity>(new AggregatorOperationException(System.Net.HttpStatusCode.InternalServerError, "Failed to retrieve sets by IDs", response.OuterException));
        }

        IEnumerable<ISetItemItrEntity> mappedSets = await _setItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ISetItemCollectionOufEntity oufEntity = await _setItemItrToOufMapper.Map(mappedSets).ConfigureAwait(false);
        return new SuccessOperationResponse<ISetItemCollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(ISetCodesItrEntity args)
    {
        ISetCodesXfrEntity xfrEntity = await _setCodesItrToXfrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _setAdapterService.SetsByCodesAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionOufEntity>(new AggregatorOperationException(System.Net.HttpStatusCode.InternalServerError, "Failed to retrieve sets by codes", response.OuterException));
        }

        IEnumerable<ISetItemItrEntity> mappedSets = await _setItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ISetItemCollectionOufEntity oufEntity = await _setItemItrToOufMapper.Map(mappedSets).ConfigureAwait(false);
        return new SuccessOperationResponse<ISetItemCollectionOufEntity>(oufEntity);
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
