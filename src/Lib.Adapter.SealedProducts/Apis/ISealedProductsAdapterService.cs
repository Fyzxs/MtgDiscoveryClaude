using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.SealedProducts.Apis;

public interface ISealedProductsAdapterService
{
    Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> GetBySetCodeAsync(
        ISealedProductsBySetCodeXfrEntity setCode,
        CancellationToken cancellationToken);
}
