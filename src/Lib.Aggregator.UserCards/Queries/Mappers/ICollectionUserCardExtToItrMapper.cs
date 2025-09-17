using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

/// <summary>
/// Maps collections of UserCardExtEntity to IUserCardItrEntity.
/// </summary>
internal interface ICollectionUserCardExtToItrMapper
{
    Task<IEnumerable<IUserCardItrEntity>> Map(IEnumerable<UserCardExtEntity> source);
}