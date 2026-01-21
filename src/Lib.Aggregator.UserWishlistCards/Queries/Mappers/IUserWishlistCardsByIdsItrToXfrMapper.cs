using System.Threading.Tasks;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Queries.Mappers;

internal interface IUserWishlistCardsByIdsItrToXfrMapper
{
    Task<IUserWishlistCardsByIdsXfrEntity> Map(IUserWishlistCardsByIdsItrEntity source);
}
