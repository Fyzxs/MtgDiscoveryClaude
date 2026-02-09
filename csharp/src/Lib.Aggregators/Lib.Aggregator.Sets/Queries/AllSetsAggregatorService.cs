using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetItems;
using Lib.Adapter.Sets.Apis;
using Lib.Aggregator.Sets.Exceptions;
using Lib.Aggregator.Sets.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.DataModels.Entities.Xfrs.Sets;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Queries;

internal sealed class AllSetsAggregatorService : IAllSetsAggregatorService
{
    private readonly ISetAdapterService _setAdapterService;
    private readonly ISetItemCollectionExtToOufMapper _collectionMapper;
    private readonly IAllSetsItrToXfrMapper _allSetsMapper;

    public AllSetsAggregatorService(ILogger logger) : this(
        new SetAdapterService(logger),
        new SetItemCollectionExtToOufMapper(),
        new AllSetsItrToXfrMapper())
    { }

    private AllSetsAggregatorService(
        ISetAdapterService setAdapterService,
        ISetItemCollectionExtToOufMapper collectionMapper,
        IAllSetsItrToXfrMapper allSetsMapper)
    {
        _setAdapterService = setAdapterService;
        _collectionMapper = collectionMapper;
        _allSetsMapper = allSetsMapper;
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> Execute(
        IAllSetsItrEntity input,
        CancellationToken cancellationToken)
    {
        IAllSetsXfrEntity allSetsXfr = await _allSetsMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _setAdapterService.AllSetsAsync(allSetsXfr, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionOufEntity>(new SetsAggregatorOperationException("Failed to retrieve all sets", response.OuterException));
        }

        ISetItemCollectionOufEntity oufEntity = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ISetItemCollectionOufEntity>(oufEntity);
    }
}
