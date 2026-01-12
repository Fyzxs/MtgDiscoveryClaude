using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

namespace Lib.Adapter.UserSealedProducts.Mappers;

/// <summary>
/// Maps between UserSealedProductExtEntity and IUserSealedProductItrEntity.
/// </summary>
internal interface IUserSealedProductMapper
{
    /// <summary>
    /// Maps from external entity to internal transfer entity.
    /// </summary>
    IUserSealedProductItrEntity Map(UserSealedProductExtEntity extEntity);

    /// <summary>
    /// Creates a ReadPointItem for reading a sealed product from Cosmos DB.
    /// </summary>
    ReadPointItem MapToReadPoint(string productUuid, string setId);
}
