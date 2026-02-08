using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CollectionCardItemExtToOufMapper : CollectionCreateMapper<ScryfallCardItemExtEntity, ICardItemItrEntity>, ICollectionCardItemExtToOufMapper
{
    public CollectionCardItemExtToOufMapper() : base(new CardItemExtToOufMapper())
    { }
}
