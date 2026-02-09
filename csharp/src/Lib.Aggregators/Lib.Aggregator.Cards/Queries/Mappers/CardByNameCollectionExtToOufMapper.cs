using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardsByName;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CardByNameCollectionExtToOufMapper : ICardByNameCollectionExtToOufMapper
{
    private readonly ICollectionCardByNameExtToOufMapper _itemMapper;

    public CardByNameCollectionExtToOufMapper() : this(new CollectionCardByNameExtToOufMapper()) { }

    private CardByNameCollectionExtToOufMapper(ICollectionCardByNameExtToOufMapper itemMapper) => _itemMapper = itemMapper;

    public async Task<ICardItemCollectionOufEntity> Map(IEnumerable<ScryfallCardByNameExtEntity> source)
    {
        IEnumerable<ICardItemOufEntity> mappedItems = await _itemMapper.Map(source).ConfigureAwait(false);

        return new CardItemCollectionOufEntity { Data = [.. mappedItems] };
    }
}
