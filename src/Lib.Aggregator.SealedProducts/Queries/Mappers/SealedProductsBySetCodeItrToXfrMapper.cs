using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Aggregator.SealedProducts.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

namespace Lib.Aggregator.SealedProducts.Queries.Mappers;

internal sealed class SealedProductsBySetCodeItrToXfrMapper : ISealedProductsBySetCodeItrToXfrMapper
{
    public ISealedProductsBySetCodeXfrEntity Map(ISealedProductsBySetCodeItrEntity source) =>
        new SetCodeXfrEntity { SetCode = source.SetCode };
}
