using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetItems;
using Lib.Adapter.Sets.Apis;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Aggregator.Sets.Exceptions;
using Lib.Aggregator.Sets.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Queries;

internal sealed class SetsByCodeAggregatorService : ISetsByCodeAggregatorService
{
    private readonly ISetAdapterService _setAdapterService;
    private readonly ISetCodesItrToXfrMapper _setCodesItrToXfrMapper;
    private readonly ISetItemCollectionExtToOufMapper _collectionMapper;

    public SetsByCodeAggregatorService(ILogger logger) : this(
        new SetAdapterService(logger),
        new SetCodesItrToXfrMapper(),
        new SetItemCollectionExtToOufMapper())
    { }

    private SetsByCodeAggregatorService(
        ISetAdapterService setAdapterService,
        ISetCodesItrToXfrMapper setCodesItrToXfrMapper,
        ISetItemCollectionExtToOufMapper collectionMapper)
    {
        _setAdapterService = setAdapterService;
        _setCodesItrToXfrMapper = setCodesItrToXfrMapper;
        _collectionMapper = collectionMapper;
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> Execute(
        ISetCodesItrEntity input,
        CancellationToken cancellationToken)
    {
        ISetCodesXfrEntity xfrEntity = await _setCodesItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _setAdapterService.SetsByCodesAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionOufEntity>(new SetsAggregatorOperationException("Failed to retrieve sets by codes", response.OuterException));
        }

        ISetItemCollectionOufEntity oufEntity = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ISetItemCollectionOufEntity>(oufEntity);
    }
}
