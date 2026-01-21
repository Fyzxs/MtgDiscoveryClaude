using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICollectionSetCardItemExtToItrMapper
{
    Task<IEnumerable<ICardItemItrEntity>> Map(IEnumerable<ScryfallSetCardItemExtEntity> source);
}
