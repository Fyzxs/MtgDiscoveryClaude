using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SealedProducts;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.Aggregator.SealedProducts.Queries.Mappers;

internal interface ICollectionSealedProductExtToOufMapper : ICreateMapper<IEnumerable<SealedProductExtEntity>, IEnumerable<ISealedProductOufEntity>>;
