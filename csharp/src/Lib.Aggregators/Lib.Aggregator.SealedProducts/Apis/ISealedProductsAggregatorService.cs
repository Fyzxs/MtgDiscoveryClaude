using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.SealedProducts.Apis;

public interface ISealedProductsAggregatorService
{
    Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> SealedProductsBySetCodeAsync(
        ISealedProductsBySetCodeItrEntity setCode);
}
