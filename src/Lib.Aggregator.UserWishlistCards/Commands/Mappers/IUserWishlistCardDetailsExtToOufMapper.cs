using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Commands.Mappers;

internal interface IUserWishlistCardDetailsExtToOufMapper
{
    Task<IUserWishlistCardDetailsOufEntity> Map(UserWishlistCardDetailsExtEntity source);
}
