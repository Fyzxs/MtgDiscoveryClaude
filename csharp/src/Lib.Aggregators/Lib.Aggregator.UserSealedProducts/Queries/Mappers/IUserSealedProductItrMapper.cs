using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Queries.Mappers;

internal interface IUserSealedProductItrMapper : ICreateMapper<UserSealedProductExtEntity, IUserSealedProductItrEntity>;
