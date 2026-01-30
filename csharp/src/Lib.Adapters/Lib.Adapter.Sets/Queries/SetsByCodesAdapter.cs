using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Adapter.Sets.Queries.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Sets.Queries;

/// <summary>
/// Retrieves sets by their codes using set code index lookup.
/// </summary>
internal sealed class SetsByCodesAdapter : ISetsByCodesAdapter
{
    private readonly ICosmosGopher _setCodeIndexGopher;
    private readonly ISetsByIdsAdapter _setsByIdsAdapter;
    private readonly ICollectionSetCodeToReadPointItemMapper _setCodeToReadPointMapper;
    private readonly ICollectionStringToSetIdsXfrMapper _stringToSetIdsXfrMapper;

    public SetsByCodesAdapter(ILogger logger) : this(
        new ScryfallSetCodeIndexGopher(logger),
        new SetsByIdsAdapter(logger),
        new CollectionSetCodeToReadPointItemMapper(),
        new CollectionStringToSetIdsXfrMapper())
    { }

    private SetsByCodesAdapter(
        ICosmosGopher setCodeIndexGopher,
        ISetsByIdsAdapter setsByIdsAdapter,
        ICollectionSetCodeToReadPointItemMapper setCodeToReadPointMapper,
        ICollectionStringToSetIdsXfrMapper stringToSetIdsXfrMapper)
    {
        _setCodeIndexGopher = setCodeIndexGopher;
        _setsByIdsAdapter = setsByIdsAdapter;
        _setCodeToReadPointMapper = setCodeToReadPointMapper;
        _stringToSetIdsXfrMapper = stringToSetIdsXfrMapper;
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> Execute([NotNull] ISetCodesXfrEntity input)
    {
        // TODO: Mapper should take the xfrEntity
        IEnumerable<string> setCodeList = input.SetCodes;
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
        return await _setsByIdsAdapter.Execute(setIdsEntity).ConfigureAwait(false);
    }
}
