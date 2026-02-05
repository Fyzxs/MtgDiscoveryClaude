using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SealedProducts;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.Adapter.SealedProducts.Queries.Mappers;

internal interface ISealedProductExtToOufMapper : ICreateMapper<SealedProductExtEntity, ISealedProductOufEntity>;
