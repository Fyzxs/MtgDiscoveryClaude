using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Commands.Mappers;

internal interface IUserWishlistCardDetailsExtToOufMapper : ICreateMapper<UserWishlistCardDetailsExtEntity, IUserWishlistCardDetailsOufEntity>;
