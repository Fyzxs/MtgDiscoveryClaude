using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

/// <summary>
/// Maps collections of UserCardExtEntity to IUserCardItrEntity.
/// </summary>
internal interface ICollectionUserCardExtToItrMapper
{
    Task<IEnumerable<IUserCardOufEntity>> Map(IEnumerable<UserCardExtEntity> source);
}
