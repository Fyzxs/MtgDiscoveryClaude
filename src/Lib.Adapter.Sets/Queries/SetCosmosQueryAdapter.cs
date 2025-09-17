using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Sets.Apis;
using Lib.Adapter.Sets.Entities;
using Lib.Adapter.Sets.Exceptions;
using Lib.Adapter.Sets.Queries.Mappers;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Sets.Queries;

/// <summary>
/// Cosmos DB implementation of the set query adapter.
/// 
/// This class handles all Cosmos DB-specific set query operations,
/// implementing the specialized ISetQueryAdapter interface.
/// The main SetAdapterService delegates to this implementation.
/// </summary>
internal sealed class SetCosmosQueryAdapter : ISetQueryAdapter
{
    private readonly ICosmosGopher _setGopher;
    private readonly ICosmosGopher _setCodeIndexGopher;
    private readonly ICosmosInquisition _allSetsInquisition;
    private readonly IScryfallSetItemToSetItemItrEntityMapper _setMapper;

    public SetCosmosQueryAdapter(ILogger logger) : this(
        new ScryfallSetItemsGopher(logger),
        new ScryfallSetCodeIndexGopher(logger),
        new AllSetItemsInquisition(logger),
        new ScryfallSetItemToSetItemItrEntityMapper())
    { }

    private SetCosmosQueryAdapter(
        ICosmosGopher setGopher,
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisition allSetsInquisition,
        IScryfallSetItemToSetItemItrEntityMapper setMapper)
    {
        _setGopher = setGopher;
        _setCodeIndexGopher = setCodeIndexGopher;
        _allSetsInquisition = allSetsInquisition;
        _setMapper = setMapper;
    }

    //TODO: Remove the Get prefix
    public async Task<IOperationResponse<IEnumerable<ISetItemItrEntity>>> GetSetsByIdsAsync([NotNull] ISetIdsItrEntity setIds)
    {
        // Extract primitives for external system interface
        IEnumerable<string> setIdList = setIds.SetIds;
        List<Task<OpResponse<ScryfallSetItem>>> tasks = [];

        foreach (string setId in setIdList)
        {
            //TODO: Technically this should be a Mapper. Take the collection in, return a collection of ReadPointItems
            ReadPointItem readPoint = new()
            {
                Id = new ProvidedCosmosItemId(setId),
                Partition = new ProvidedPartitionKeyValue(setId)
            };
            tasks.Add(_setGopher.ReadAsync<ScryfallSetItem>(readPoint));
        }

        OpResponse<ScryfallSetItem>[] responses = await Task.WhenAll(tasks).ConfigureAwait(false);

        //TODO: Technically this should be a Mapper. Take the collection in, return a collection of ISetItemItrEntity
        List<ISetItemItrEntity> successfulSets = [];
        foreach (OpResponse<ScryfallSetItem> response in responses.Where(r => r.IsSuccessful()))
        {
            ISetItemItrEntity mapped = await _setMapper.Map(response.Value).ConfigureAwait(false);
            if (mapped != null) successfulSets.Add(mapped);
        }

        return new SuccessOperationResponse<IEnumerable<ISetItemItrEntity>>(successfulSets);
    }

    public async Task<IOperationResponse<IEnumerable<ISetItemItrEntity>>> GetSetsByCodesAsync([NotNull] ISetCodesItrEntity setCodes)
    {
        // Extract primitives for external system interface
        IEnumerable<string> setCodeList = setCodes.SetCodes;
        List<Task<OpResponse<ScryfallSetCodeIndexItem>>> indexTasks = [];

        foreach (string setCode in setCodeList)
        {
            ReadPointItem readPoint = new()
            {
                Id = new ProvidedCosmosItemId(setCode),
                Partition = new ProvidedPartitionKeyValue(setCode)
            };
            indexTasks.Add(_setCodeIndexGopher.ReadAsync<ScryfallSetCodeIndexItem>(readPoint));
        }

        OpResponse<ScryfallSetCodeIndexItem>[] indexResponses = await Task.WhenAll(indexTasks).ConfigureAwait(false);

        List<string> setIds = [.. indexResponses
            .Where(r => r.IsSuccessful())
            .Select(r => r.Value.SetId)];

        if (setIds.Count == 0)
        {
            return new SuccessOperationResponse<IEnumerable<ISetItemItrEntity>>([]);
        }

        //TODO: Should be a mapper
        SetIdsItrEntity setIdsEntity = new() { SetIds = setIds };
        return await GetSetsByIdsAsync(setIdsEntity).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<ISetItemItrEntity>>> GetAllSetsAsync()
    {
        OpResponse<IEnumerable<ScryfallSetItem>> response = await _allSetsInquisition
            .QueryAsync<ScryfallSetItem>(CancellationToken.None)
            .ConfigureAwait(false);

        if (response.IsSuccessful() is false)
        {
            return new FailureOperationResponse<IEnumerable<ISetItemItrEntity>>(
                new SetAdapterException("Failed to retrieve all sets", response.Exception()));
        }

        //TODO: Should be a mapper
        List<ISetItemItrEntity> sets = [];
        foreach (ScryfallSetItem item in response.Value)
        {
            ISetItemItrEntity mapped = await _setMapper.Map(item).ConfigureAwait(false);
            if (mapped != null) sets.Add(mapped);
        }

        return new SuccessOperationResponse<IEnumerable<ISetItemItrEntity>>(sets);
    }
}
