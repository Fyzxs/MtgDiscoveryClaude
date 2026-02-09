using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

/// <summary>
/// Maps collections of UserCardExtEntity to IUserCardOufEntity.
/// </summary>
internal interface ICollectionUserCardExtToOufMapper : ICreateMapper<IEnumerable<UserCardExtEntity>, IEnumerable<IUserCardOufEntity>>
{
}
