using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetItems;
using Lib.Aggregator.Sets.Entities;
using Lib.Shared.DataModels.Entities.Oufs.Sets;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal sealed class SetItemCollectionExtToOufMapper : ISetItemCollectionExtToOufMapper
{
    private readonly ICollectionSetItemExtToOufMapper _itemMapper;

    public SetItemCollectionExtToOufMapper() : this(new CollectionSetItemExtToOufMapper()) { }

    private SetItemCollectionExtToOufMapper(ICollectionSetItemExtToOufMapper itemMapper) => _itemMapper = itemMapper;

    public async Task<ISetItemCollectionOufEntity> Map(IEnumerable<ScryfallSetItemExtEntity> source)
    {
        IEnumerable<ISetItemOufEntity> mappedItems = await _itemMapper.Map(source).ConfigureAwait(false);

        return new SetItemCollectionOufEntity { Data = [.. mappedItems] };
    }
}
