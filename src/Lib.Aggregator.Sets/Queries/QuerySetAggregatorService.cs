using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Sets.Apis;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Aggregator.Sets.Apis;
using Lib.Aggregator.Sets.Entities;
using Lib.Aggregator.Sets.Exceptions;
using Lib.Aggregator.Sets.Queries.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Queries;

internal sealed class QuerySetAggregatorService : ISetAggregatorService
{
    private readonly ISetAdapterService _setAdapterService;
    private readonly ICollectionSetItemExtToItrMapper _setItemMapper;
    private readonly ISetIdsItrToXfrMapper _setIdsItrToXfrMapper;
    private readonly ISetCodesItrToXfrMapper _setCodesItrToXfrMapper;

    public QuerySetAggregatorService(ILogger logger) : this(
        new SetAdapterService(logger),
        new CollectionSetItemExtToItrMapper(),
        new SetIdsItrToXfrMapper(),
        new SetCodesItrToXfrMapper())
    { }

    private QuerySetAggregatorService(
        ISetAdapterService setAdapterService,
        ICollectionSetItemExtToItrMapper setItemMapper,
        ISetIdsItrToXfrMapper setIdsItrToXfrMapper,
        ISetCodesItrToXfrMapper setCodesItrToXfrMapper)
    {
        _setAdapterService = setAdapterService;
        _setItemMapper = setItemMapper;
        _setIdsItrToXfrMapper = setIdsItrToXfrMapper;
        _setCodesItrToXfrMapper = setCodesItrToXfrMapper;
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsAsync(ISetIdsItrEntity args)
    {
        ISetIdsXfrEntity xfrEntity = await _setIdsItrToXfrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _setAdapterService.GetSetsByIdsAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionItrEntity>(new AggregatorOperationException(System.Net.HttpStatusCode.InternalServerError, "Failed to retrieve sets by IDs", response.OuterException));
        }

        IEnumerable<ISetItemItrEntity> mappedSets = await _setItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ICollection<ISetItemItrEntity> sets = [.. mappedSets];
        return new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntity { Data = sets });
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesItrEntity args)
    {
        ISetCodesXfrEntity xfrEntity = await _setCodesItrToXfrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _setAdapterService.GetSetsByCodesAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionItrEntity>(new AggregatorOperationException(System.Net.HttpStatusCode.InternalServerError, "Failed to retrieve sets by codes", response.OuterException));
        }

        IEnumerable<ISetItemItrEntity> mappedSets = await _setItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ICollection<ISetItemItrEntity> sets = [.. mappedSets];
        return new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntity { Data = sets });
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> AllSetsAsync()
    {
        IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _setAdapterService.GetAllSetsAsync().ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionItrEntity>(new AggregatorOperationException(System.Net.HttpStatusCode.InternalServerError, "Failed to retrieve all sets", response.OuterException));
        }

        IEnumerable<ISetItemItrEntity> mappedSets = await _setItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ICollection<ISetItemItrEntity> sets = [.. mappedSets];
        return new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntity { Data = sets });
    }
}
