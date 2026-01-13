using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.Adapter.UserSealedProducts.Mappers;

/// <summary>
/// Maps UserSealedProductExtEntity to IUserSealedProductOufEntity for response output.
/// </summary>
public interface IUserSealedProductOufMapper : ICreateMapper<UserSealedProductExtEntity, IUserSealedProductOufEntity>;
