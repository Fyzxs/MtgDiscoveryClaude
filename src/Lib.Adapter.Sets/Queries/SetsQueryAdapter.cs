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
using Lib.Adapter.Sets.Queries.Mappers;
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
internal sealed class SetsQueryAdapter : ISetQueryAdapter
{
    private readonly ICosmosGopher _setGopher;
    private readonly ICosmosGopher _setCodeIndexGopher;
    private readonly ICosmosInquisition _allSetsInquisition;
    private readonly ICollectionSetIdToReadPointItemMapper _setIdToReadPointMapper;
    private readonly ICollectionSetCodeToReadPointItemMapper _setCodeToReadPointMapper;
    private readonly ICollectionStringToSetIdsXfrMapper _stringToSetIdsXfrMapper;

    public SetsQueryAdapter(ILogger logger) : this(
        new ScryfallSetItemsGopher(logger),
        new ScryfallSetCodeIndexGopher(logger),
        new AllSetItemsInquisition(logger),
        new CollectionSetIdToReadPointItemMapper(),
        new CollectionSetCodeToReadPointItemMapper(),
        new CollectionStringToSetIdsXfrMapper())
    { }

    private SetsQueryAdapter(
        ICosmosGopher setGopher,
        ICosmosGopher setCodeIndexGopher,
        ICosmosInquisition allSetsInquisition,
        ICollectionSetIdToReadPointItemMapper setIdToReadPointMapper,
        ICollectionSetCodeToReadPointItemMapper setCodeToReadPointMapper,
        ICollectionStringToSetIdsXfrMapper stringToSetIdsXfrMapper)
    {
        _setGopher = setGopher;
        _setCodeIndexGopher = setCodeIndexGopher;
        _allSetsInquisition = allSetsInquisition;
        _setIdToReadPointMapper = setIdToReadPointMapper;
        _setCodeToReadPointMapper = setCodeToReadPointMapper;
        _stringToSetIdsXfrMapper = stringToSetIdsXfrMapper;
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> SetsByIdsAsync([NotNull] ISetIdsXfrEntity setIds)
    {
        IEnumerable<string> setIdList = setIds.SetIds;
        ICollection<ReadPointItem> readPoints = await _setIdToReadPointMapper.Map(setIdList).ConfigureAwait(false);

        List<Task<OpResponse<ScryfallSetItemExtEntity>>> tasks = [];
        foreach (ReadPointItem readPoint in readPoints)
        {
            tasks.Add(_setGopher.ReadAsync<ScryfallSetItemExtEntity>(readPoint));
        }

        OpResponse<ScryfallSetItemExtEntity>[] responses = await Task.WhenAll(tasks).ConfigureAwait(false);

        IEnumerable<ScryfallSetItemExtEntity> successfulSets = responses
            .Where(task => task.IsSuccessful())
            .Select(task => task.Value)
            .Where(set => set is not null);

        return new SuccessOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>(successfulSets);
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> SetsByCodesAsync([NotNull] ISetCodesXfrEntity setCodes)
    {
        // Extract primitives for external system interface
        IEnumerable<string> setCodeList = setCodes.SetCodes;
        ICollection<ReadPointItem> readPoints = await _setCodeToReadPointMapper.Map(setCodeList).ConfigureAwait(false);

        List<Task<OpResponse<ScryfallSetCodeIndexExtEntity>>> indexTasks = [];
        foreach (ReadPointItem readPoint in readPoints)
        {
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

        ISetIdsXfrEntity setIdsEntity = await _stringToSetIdsXfrMapper.Map(setIds).ConfigureAwait(false);
        return await SetsByIdsAsync(setIdsEntity).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> AllSetsAsync()
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
