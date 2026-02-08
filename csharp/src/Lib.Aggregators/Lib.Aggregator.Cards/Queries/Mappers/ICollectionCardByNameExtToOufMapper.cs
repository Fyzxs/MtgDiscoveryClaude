using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardsByName;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICollectionCardByNameExtToOufMapper : ICreateMapper<IEnumerable<ScryfallCardByNameExtEntity>, IEnumerable<ICardItemItrEntity>>
{
}
