using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Queries.Mappers;

internal interface ICollectionUserWishlistCardExtToOufMapper : ICreateMapper<IEnumerable<UserWishlistCardExtEntity>, IEnumerable<IUserWishlistCardOufEntity>>;
