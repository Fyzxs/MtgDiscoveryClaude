using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Aggregator.Sets.Apis;
using Lib.Aggregator.Sets.Entities;
using Lib.Aggregator.Sets.Exceptions;
using Lib.Aggregator.Sets.Queries.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Queries;

internal sealed class QuerySetAggregatorService : ISetAggregatorService
{
    private readonly ICosmosGopher _setGopher;
    private readonly ICosmosGopher _setCodeIndexGopher;
    private readonly ICosmosInquisitor _setInquisitor;
    private readonly QuerySetsIdsToReadPointItemsMapper _idsMapper;
    private readonly QuerySetsCodesToReadPointItemsMapper _codesMapper;
    private readonly ScryfallSetItemToSetItemItrEntityMapper _setMapper;

    public QuerySetAggregatorService(ILogger logger) : this(
        new ScryfallSetItemsGopher(logger),
        new ScryfallSetCodeIndexGopher(logger),
        new ScryfallSetItemsInquisitor(logger),
        new QuerySetsIdsToReadPointItemsMapper(),
        new QuerySetsCodesToReadPointItemsMapper(),
        new ScryfallSetItemToSetItemItrEntityMapper())
    {
    }

    private QuerySetAggregatorService(
        ICosmosGopher setGopher,
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisitor setInquisitor,
        QuerySetsIdsToReadPointItemsMapper idsMapper,
        QuerySetsCodesToReadPointItemsMapper codesMapper,
        ScryfallSetItemToSetItemItrEntityMapper setMapper)
    {
        _setGopher = setGopher;
        _setCodeIndexGopher = setCodeIndexGopher;
        _setInquisitor = setInquisitor;
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
            .Where(r => r.IsSuccessful() && r.Value != null)
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
        /*
         * TODO: qgil/20250824need
         * Create an AllSetsQueryDefinition
         * Create an AllSetsInquisitor to couple with the AllSetsQueryDefinition
         * return result, no exception.
         */
        QueryDefinition queryDefinition = new("SELECT * FROM c");
        OpResponse<IEnumerable<ScryfallSetItem>> response = await _setInquisitor.QueryAsync<ScryfallSetItem>(
            queryDefinition,
            PartitionKey.None).ConfigureAwait(false);

        if (response.IsSuccessful() is false)
        {
            return new FailureOperationResponse<ISetItemCollectionItrEntity>(
                new AggregatorOperationException("Failed to retrieve all sets", response.Exception()));
        }

        ICollection<ISetItemItrEntity> sets = response.Value
            .Select(item => _setMapper.Map(item))
            .Where(set => set != null)
            .ToList();

        return new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntity { Data = sets });
    }
}
