using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Aggregator.Sets.Apis;
using Lib.Aggregator.Sets.Entities;
using Lib.Aggregator.Sets.Exceptions;
using Lib.Aggregator.Sets.Queries.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Queries;

internal sealed class QuerySetAggregatorService : ISetAggregatorService
{
    private readonly ICosmosGopher _setGopher;
    private readonly ICosmosGopher _setCodeIndexGopher;
    private readonly ICosmosInquisition _allSetsInquisition;
    private readonly QuerySetsIdsToReadPointItemsMapper _idsMapper;
    private readonly QuerySetsCodesToReadPointItemsMapper _codesMapper;
    private readonly ScryfallSetItemToSetItemItrEntityMapper _setMapper;

    public QuerySetAggregatorService(ILogger logger) : this(
        new ScryfallSetItemsGopher(logger),
        new ScryfallSetCodeIndexGopher(logger),
        new SelectAllSetItemsInquisition(logger),
        new QuerySetsIdsToReadPointItemsMapper(),
        new QuerySetsCodesToReadPointItemsMapper(),
        new ScryfallSetItemToSetItemItrEntityMapper())
    {
    }

    private QuerySetAggregatorService(
        ICosmosGopher setGopher,
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisition allSetsInquisition,
        QuerySetsIdsToReadPointItemsMapper idsMapper,
        QuerySetsCodesToReadPointItemsMapper codesMapper,
        ScryfallSetItemToSetItemItrEntityMapper setMapper)
    {
        _setGopher = setGopher;
        _setCodeIndexGopher = setCodeIndexGopher;
        _allSetsInquisition = allSetsInquisition;
        _idsMapper = idsMapper;
        _codesMapper = codesMapper;
        _setMapper = setMapper;
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsAsync(ISetIdsItrEntity args)
    {
        IEnumerable<ReadPointItem> readPointItems = _idsMapper.Map(args);
        List<Task<OpResponse<ScryfallSetItem>>> tasks = [];

        tasks.AddRange(readPointItems.Select(readPointItem => _setGopher.ReadAsync<ScryfallSetItem>(readPointItem)));

        OpResponse<ScryfallSetItem>[] responses = await Task.WhenAll(tasks).ConfigureAwait(false);

        ICollection<ISetItemItrEntity> successfulSets = responses
            .Where(r => r.IsSuccessful())
            .Select(r => _setMapper.Map(r.Value))
            .Where(set => set != null)
            .ToList();

        return new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntity { Data = successfulSets });
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesItrEntity args)
    {
        IEnumerable<ReadPointItem> readPointItems = _codesMapper.Map(args);
        List<Task<OpResponse<ScryfallSetCodeIndexItem>>> indexTasks = [];

        indexTasks.AddRange(readPointItems.Select(readPointItem => _setCodeIndexGopher.ReadAsync<ScryfallSetCodeIndexItem>(readPointItem)));

        OpResponse<ScryfallSetCodeIndexItem>[] indexResponses = await Task.WhenAll(indexTasks).ConfigureAwait(false);

        List<string> setIds = indexResponses
            .Where(r => r.IsSuccessful())
            .Select(r => r.Value.SetId)
            .ToList();

        if (setIds.Count == 0)
        {
            return new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntity { Data = [] });
        }

        SetIdsItrEntity setIdsEntity = new() { SetIds = setIds };
        return await SetsAsync(setIdsEntity).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> AllSetsAsync()
    {
        OpResponse<IEnumerable<ScryfallSetItem>> response = await _allSetsInquisition.QueryAsync<ScryfallSetItem>(CancellationToken.None).ConfigureAwait(false);

        if (response.IsSuccessful() is false)
        {
            return new FailureOperationResponse<ISetItemCollectionItrEntity>(new AggregatorOperationException(response.StatusCode, "Failed to retrieve all sets", response.Exception()));
        }

        ICollection<ISetItemItrEntity> sets = response.Value
            .Select(item => _setMapper.Map(item))
            .ToList();

        return new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntity { Data = sets });
    }
}
