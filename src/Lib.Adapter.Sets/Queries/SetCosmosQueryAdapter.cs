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
    private readonly ScryfallSetItemToSetItemItrEntityMapper _setMapper;

    public SetCosmosQueryAdapter(ILogger logger) : this(
        new ScryfallSetItemsGopher(logger),
        new ScryfallSetCodeIndexGopher(logger),
        new SelectAllSetItemsInquisition(logger),
        new ScryfallSetItemToSetItemItrEntityMapper())
    { }

    private SetCosmosQueryAdapter(
        ICosmosGopher setGopher,
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisition allSetsInquisition,
        ScryfallSetItemToSetItemItrEntityMapper setMapper)
    {
        _setGopher = setGopher;
        _setCodeIndexGopher = setCodeIndexGopher;
        _allSetsInquisition = allSetsInquisition;
        _setMapper = setMapper;
    }

    public async Task<IOperationResponse<IEnumerable<ISetItemItrEntity>>> GetSetsByIdsAsync([NotNull] ISetIdsItrEntity setIds)
    {
        // Extract primitives for external system interface
        IEnumerable<string> setIdList = setIds.SetIds;
        List<Task<OpResponse<ScryfallSetItem>>> tasks = [];

        foreach (string setId in setIdList)
        {
            ReadPointItem readPoint = new()
            {
                Id = new ProvidedCosmosItemId(setId),
                Partition = new ProvidedPartitionKeyValue(setId)
            };
            tasks.Add(_setGopher.ReadAsync<ScryfallSetItem>(readPoint));
        }

        OpResponse<ScryfallSetItem>[] responses = await Task.WhenAll(tasks).ConfigureAwait(false);

        IEnumerable<ISetItemItrEntity> successfulSets = responses
            .Where(r => r.IsSuccessful())
            .Select(r => _setMapper.Map(r.Value))
            .Where(set => set != null);

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

        // Create internal entity with found set IDs and recursively call GetSetsByIdsAsync
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

        IEnumerable<ISetItemItrEntity> sets = response.Value
            .Select(item => _setMapper.Map(item))
            .Where(set => set != null);

        return new SuccessOperationResponse<IEnumerable<ISetItemItrEntity>>(sets);
    }
}
