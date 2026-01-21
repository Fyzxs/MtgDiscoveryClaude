using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.Adapter.SealedProducts.Queries.Mappers;

internal interface ISealedProductExtToOufMapper
{
    ISealedProductOufEntity Map(SealedProductExtEntity source);
}
