using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.ArtistCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class CollectionArtistCardExtToOufMapper : CollectionCreateMapper<ScryfallArtistCardExtEntity, ICardItemOufEntity>, ICollectionArtistCardExtToOufMapper
{
    public CollectionArtistCardExtToOufMapper() : base(new ArtistCardExtToOufEntityMapper())
    { }
}
