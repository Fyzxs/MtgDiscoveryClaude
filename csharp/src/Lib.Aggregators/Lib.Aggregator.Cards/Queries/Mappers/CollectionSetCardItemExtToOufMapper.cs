using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CollectionSetCardItemExtToOufMapper : CollectionCreateMapper<ScryfallSetCardItemExtEntity, ICardItemItrEntity>, ICollectionSetCardItemExtToOufMapper
{
    public CollectionSetCardItemExtToOufMapper() : base(new SetCardItemExtToOufMapper())
    { }
}
