using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Sets;

namespace Lib.Aggregator.Sets.Queries.Mappers;

/// <summary>
/// Maps collections of ScryfallSetItemExtEntity to ISetItemOufEntity.
/// </summary>
internal interface ICollectionSetItemExtToOufMapper : ICreateMapper<IEnumerable<ScryfallSetItemExtEntity>, IEnumerable<ISetItemOufEntity>>
{
}
