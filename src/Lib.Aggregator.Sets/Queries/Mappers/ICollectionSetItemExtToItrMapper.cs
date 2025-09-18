using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Sets.Queries.Mappers;

/// <summary>
/// Maps collections of ScryfallSetItemExtEntity to ISetItemItrEntity.
/// </summary>
internal interface ICollectionSetItemExtToItrMapper
{
    Task<IEnumerable<ISetItemItrEntity>> Map(IEnumerable<ScryfallSetItemExtEntity> source);
}
