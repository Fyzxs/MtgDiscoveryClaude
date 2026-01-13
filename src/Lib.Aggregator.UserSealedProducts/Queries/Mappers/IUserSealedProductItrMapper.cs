using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Patterns.Creation;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Queries.Mappers;

internal interface IUserSealedProductItrMapper : ICreateMapper<UserSealedProductExtEntity, IUserSealedProductItrEntity>;
