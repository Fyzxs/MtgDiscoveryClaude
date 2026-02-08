using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICollectionSetCardItemExtToOufMapper : ICreateMapper<IEnumerable<ScryfallSetCardItemExtEntity>, IEnumerable<ICardItemOufEntity>>
{
}
