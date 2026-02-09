using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.ArtistCards;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistCardCollectionExtToOufMapper : IArtistCardCollectionExtToOufMapper
{
    private readonly ICollectionArtistCardExtToOufMapper _itemMapper;

    public ArtistCardCollectionExtToOufMapper() : this(new CollectionArtistCardExtToOufMapper()) { }

    private ArtistCardCollectionExtToOufMapper(ICollectionArtistCardExtToOufMapper itemMapper) => _itemMapper = itemMapper;

    public async Task<ICardItemCollectionOufEntity> Map(IEnumerable<ScryfallArtistCardExtEntity> source)
    {
        IEnumerable<ICardItemOufEntity> mappedItems = await _itemMapper.Map(source).ConfigureAwait(false);

        return new CardItemCollectionOufEntity { Data = [.. mappedItems] };
    }
}
