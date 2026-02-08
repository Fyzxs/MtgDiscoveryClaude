using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetCards;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class SetCardItemCollectionExtToOufMapper : ISetCardItemCollectionExtToOufMapper
{
    private readonly ICollectionSetCardItemExtToOufMapper _itemMapper;

    public SetCardItemCollectionExtToOufMapper() : this(new CollectionSetCardItemExtToOufMapper()) { }

    private SetCardItemCollectionExtToOufMapper(ICollectionSetCardItemExtToOufMapper itemMapper) => _itemMapper = itemMapper;

    public async Task<ICardItemCollectionOufEntity> Map(IEnumerable<ScryfallSetCardItemExtEntity> source)
    {
        IEnumerable<ICardItemOufEntity> mappedItems = await _itemMapper.Map(source).ConfigureAwait(false);

        return new CardItemCollectionOufEntity { Data = [.. mappedItems] };
    }
}
