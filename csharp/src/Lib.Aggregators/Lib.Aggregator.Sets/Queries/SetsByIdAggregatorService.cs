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

internal sealed class SetsByIdAggregatorService : ISetsByIdAggregatorService
{
    private readonly ISetAdapterService _setAdapterService;
    private readonly ISetIdsItrToXfrMapper _setIdsItrToXfrMapper;
    private readonly ISetItemCollectionExtToOufMapper _collectionMapper;

    public SetsByIdAggregatorService(ILogger logger) : this(
        new SetAdapterService(logger),
        new SetIdsItrToXfrMapper(),
        new SetItemCollectionExtToOufMapper())
    { }

    private SetsByIdAggregatorService(
        ISetAdapterService setAdapterService,
        ISetIdsItrToXfrMapper setIdsItrToXfrMapper,
        ISetItemCollectionExtToOufMapper collectionMapper)
    {
        _setAdapterService = setAdapterService;
        _setIdsItrToXfrMapper = setIdsItrToXfrMapper;
        _collectionMapper = collectionMapper;
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> Execute(
        ISetIdsItrEntity input,
        CancellationToken cancellationToken)
    {
        ISetIdsXfrEntity xfrEntity = await _setIdsItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _setAdapterService.SetsByIdsAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionOufEntity>(new SetsAggregatorOperationException("Failed to retrieve sets by IDs", response.OuterException));
        }

        ISetItemCollectionOufEntity oufEntity = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ISetItemCollectionOufEntity>(oufEntity);
    }
}
