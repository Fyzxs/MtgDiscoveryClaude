using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Queries.Mappers;

internal interface ICollectionUserWishlistCardExtToOufMapper //TODO: Implement the interface all other mappers implement
{
    Task<IEnumerable<IUserWishlistCardOufEntity>> Map(IEnumerable<UserWishlistCardExtEntity> source);
}
