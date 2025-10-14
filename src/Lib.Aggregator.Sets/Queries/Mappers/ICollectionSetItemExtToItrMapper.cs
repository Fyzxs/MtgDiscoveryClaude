using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Sets.Queries.Mappers;

/// <summary>
/// Maps collections of ScryfallSetItemExtEntity to ISetItemItrEntity.
/// </summary>
internal interface ICollectionSetItemExtToItrMapper : ICreateMapper<IEnumerable<ScryfallSetItemExtEntity>, IEnumerable<ISetItemItrEntity>>
{
}
