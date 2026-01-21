using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

namespace Lib.Aggregator.SealedProducts.Queries.Mappers;

internal interface ISealedProductsBySetCodeItrToXfrMapper : ICreateMapper<ISealedProductsBySetCodeItrEntity, ISealedProductsBySetCodeXfrEntity>;
