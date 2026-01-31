using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

/// <summary>
/// Maps collections of UserCardExtEntity to IUserCardItrEntity.
/// </summary>
internal interface ICollectionUserCardExtToItrMapper : ICreateMapper<IEnumerable<UserCardExtEntity>, IEnumerable<IUserCardOufEntity>>
{
}
