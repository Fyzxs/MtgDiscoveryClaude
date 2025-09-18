using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Sets.Apis;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Adapter.Sets.Exceptions;
using Lib.Adapter.Sets.Queries.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
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

    public SetCosmosQueryAdapter(ILogger logger) : this(
        new ScryfallSetItemsGopher(logger),
        new ScryfallSetCodeIndexGopher(logger),
        new AllSetItemsInquisition(logger))
    { }

    private SetCosmosQueryAdapter(
        ICosmosGopher setGopher,
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisition allSetsInquisition)
    {
        _setGopher = setGopher;
        _setCodeIndexGopher = setCodeIndexGopher;
        _allSetsInquisition = allSetsInquisition;
    }

    //TODO: Remove the Get prefix
    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> GetSetsByIdsAsync([NotNull] ISetIdsXfrEntity setIds)
    {
        // Extract primitives for external system interface
        IEnumerable<string> setIdList = setIds.SetIds;
        List<Task<OpResponse<ScryfallSetItemExtEntity>>> tasks = [];

        foreach (string setId in setIdList)
        {
            //TODO: Technically this should be a Mapper. Take the collection in, return a collection of ReadPointItems
            ReadPointItem readPoint = new()
            {
                Id = new ProvidedCosmosItemId(setId),
                Partition = new ProvidedPartitionKeyValue(setId)
            };
            tasks.Add(_setGopher.ReadAsync<ScryfallSetItemExtEntity>(readPoint));
        }

        OpResponse<ScryfallSetItemExtEntity>[] responses = await Task.WhenAll(tasks).ConfigureAwait(false);

        IEnumerable<ScryfallSetItemExtEntity> successfulSets = responses
            .Where(task => task.IsSuccessful())
            .Select(task => task.Value)
            .Where(set => set is not null);

        return new SuccessOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>(successfulSets);
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> GetSetsByCodesAsync([NotNull] ISetCodesXfrEntity setCodes)
    {
        // Extract primitives for external system interface
        IEnumerable<string> setCodeList = setCodes.SetCodes;
        List<Task<OpResponse<ScryfallSetCodeIndexExtEntity>>> indexTasks = [];

        foreach (string setCode in setCodeList)
        {
            ReadPointItem readPoint = new()
            {
                Id = new ProvidedCosmosItemId(setCode),
                Partition = new ProvidedPartitionKeyValue(setCode)
            };
            indexTasks.Add(_setCodeIndexGopher.ReadAsync<ScryfallSetCodeIndexExtEntity>(readPoint));
        }

        OpResponse<ScryfallSetCodeIndexExtEntity>[] indexResponses = await Task.WhenAll(indexTasks).ConfigureAwait(false);

        List<string> setIds = [.. indexResponses
            .Where(r => r.IsSuccessful())
            .Select(r => r.Value.SetId)];

        if (setIds.Count == 0)
        {
            return new SuccessOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>([]);
        }

        //TODO: Should be a mapper
        ISetIdsXfrEntity setIdsEntity = new SetIdsXfrEntity { SetIds = setIds };
        return await GetSetsByIdsAsync(setIdsEntity).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> GetAllSetsAsync()
    {
        OpResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _allSetsInquisition
            .QueryAsync<ScryfallSetItemExtEntity>(CancellationToken.None)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>(
                new SetAdapterException("Failed to retrieve all sets", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>(response.Value);
    }
}
