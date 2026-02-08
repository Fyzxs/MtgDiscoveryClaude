using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardItems;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardItemCollectionExtToOufMapper : ICardItemCollectionExtToOufMapper
{
    private readonly ICollectionCardItemExtToOufMapper _itemMapper;

    public CardItemCollectionExtToOufMapper() : this(new CollectionCardItemExtToOufMapper()) { }

    private CardItemCollectionExtToOufMapper(ICollectionCardItemExtToOufMapper itemMapper) => _itemMapper = itemMapper;

    public async Task<ICardItemCollectionOufEntity> Map(IEnumerable<ScryfallCardItemExtEntity> source)
    {
        IEnumerable<ICardItemOufEntity> mappedItems = await _itemMapper.Map(source).ConfigureAwait(false);

        return new CardItemCollectionOufEntity { Data = [.. mappedItems] };
    }
}
