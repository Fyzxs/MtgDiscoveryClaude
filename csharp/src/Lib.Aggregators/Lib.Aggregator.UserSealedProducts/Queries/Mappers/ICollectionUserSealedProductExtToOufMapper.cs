using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Queries.Mappers;

internal interface ICollectionUserSealedProductExtToOufMapper : ICreateMapper<IEnumerable<UserSealedProductExtEntity>, IEnumerable<IUserSealedProductOufEntity>>;
