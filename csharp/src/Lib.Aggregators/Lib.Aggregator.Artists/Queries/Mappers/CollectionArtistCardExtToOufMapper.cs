using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.ArtistCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class CollectionArtistCardExtToOufMapper : CollectionCreateMapper<ScryfallArtistCardExtEntity, ICardItemItrEntity>, ICollectionArtistCardExtToOufMapper
{
    public CollectionArtistCardExtToOufMapper() : base(new ArtistCardExtToOufEntityMapper())
    { }
}
