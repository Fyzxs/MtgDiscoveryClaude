using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Commands.Mappers;

internal interface IUserWishlistCardExtToOufEntityMapper
{
    Task<IUserWishlistCardOufEntity> Map(UserWishlistCardExtEntity source);
}
