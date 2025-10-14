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

namespace Lib.Aggregator.Sets.Queries.SetsByCode;

internal sealed class SetsByCodeAggregatorService : ISetsByCodeAggregatorService
{
    private readonly ISetAdapterService _setAdapterService;
    private readonly ICollectionSetItemExtToItrMapper _setItemMapper;
    private readonly ISetCodesItrToXfrMapper _setCodesItrToXfrMapper;
    private readonly ICollectionSetItemItrToOufMapper _setItemItrToOufMapper;

    public SetsByCodeAggregatorService(ILogger logger) : this(
        new SetAdapterService(logger),
        new CollectionSetItemExtToItrMapper(),
        new SetCodesItrToXfrMapper(),
        new CollectionSetItemItrToOufMapper())
    { }

    private SetsByCodeAggregatorService(
        ISetAdapterService setAdapterService,
        ICollectionSetItemExtToItrMapper setItemMapper,
        ISetCodesItrToXfrMapper setCodesItrToXfrMapper,
        ICollectionSetItemItrToOufMapper setItemItrToOufMapper)
    {
        _setAdapterService = setAdapterService;
        _setItemMapper = setItemMapper;
        _setCodesItrToXfrMapper = setCodesItrToXfrMapper;
        _setItemItrToOufMapper = setItemItrToOufMapper;
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

}
