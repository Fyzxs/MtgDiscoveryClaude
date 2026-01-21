using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

namespace Lib.Adapter.UserSealedProducts.Mappers;

/// <summary>
/// Maps UserSealedProductExtEntity to IUserSealedProductItrEntity for query results.
/// </summary>
public interface IUserSealedProductItrMapper : ICreateMapper<UserSealedProductExtEntity, IUserSealedProductItrEntity>;
