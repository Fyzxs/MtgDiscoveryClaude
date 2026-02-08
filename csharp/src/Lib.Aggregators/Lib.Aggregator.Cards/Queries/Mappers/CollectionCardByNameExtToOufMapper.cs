using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardsByName;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CollectionCardByNameExtToOufMapper : CollectionCreateMapper<ScryfallCardByNameExtEntity, ICardItemOufEntity>, ICollectionCardByNameExtToOufMapper
{
    public CollectionCardByNameExtToOufMapper() : base(new CardByNameExtToOufMapper())
    { }
}
