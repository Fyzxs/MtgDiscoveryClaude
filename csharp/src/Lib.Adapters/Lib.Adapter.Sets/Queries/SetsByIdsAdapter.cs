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
/// Retrieves sets by their IDs using Cosmos DB point reads.
/// </summary>
internal sealed class SetsByIdsAdapter : ISetsByIdsAdapter
{
    private readonly ICosmosGopher _setGopher;
    private readonly ICollectionSetIdToReadPointItemMapper _setIdToReadPointMapper;

    public SetsByIdsAdapter(ILogger logger) : this(
        new ScryfallSetItemsGopher(logger),
        new CollectionSetIdToReadPointItemMapper())
    { }

    private SetsByIdsAdapter(
        ICosmosGopher setGopher,
        ICollectionSetIdToReadPointItemMapper setIdToReadPointMapper)
    {
        _setGopher = setGopher;
        _setIdToReadPointMapper = setIdToReadPointMapper;
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> Execute([NotNull] ISetIdsXfrEntity input)
    {
        IEnumerable<string> setIdList = input.SetIds;
        ICollection<ReadPointItem> readPoints = await _setIdToReadPointMapper.Map(setIdList).ConfigureAwait(false);

        List<Task<OpResponse<ScryfallSetItemExtEntity>>> tasks = [];
        foreach (ReadPointItem readPoint in readPoints)
        {
            tasks.Add(_setGopher.ReadAsync<ScryfallSetItemExtEntity>(readPoint));
        }

        OpResponse<ScryfallSetItemExtEntity>[] responses = await Task.WhenAll(tasks).ConfigureAwait(false);

        //TODO: If this is an object; it can do this filtering internally
        IEnumerable<ScryfallSetItemExtEntity> successfulSets = responses
            .Where(task => task.IsSuccessful())
            .Select(task => task.Value)
            .Where(set => set is not null);

        return new SuccessOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>(successfulSets);
    }
}
