using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICollectionCardItemExtToOufMapper : ICreateMapper<IEnumerable<ScryfallCardItemExtEntity>, IEnumerable<ICardItemItrEntity>>
{
}
