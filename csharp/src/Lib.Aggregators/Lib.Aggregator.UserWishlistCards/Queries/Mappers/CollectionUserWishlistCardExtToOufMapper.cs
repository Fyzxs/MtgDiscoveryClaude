using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Aggregator.UserWishlistCards.Commands.Mappers;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Queries.Mappers;

internal sealed class CollectionUserWishlistCardExtToOufMapper : CollectionCreateMapper<UserWishlistCardExtEntity, IUserWishlistCardOufEntity>, ICollectionUserWishlistCardExtToOufMapper
{
    public CollectionUserWishlistCardExtToOufMapper() : base(new UserWishlistCardExtToOufEntityMapper())
    { }
}
