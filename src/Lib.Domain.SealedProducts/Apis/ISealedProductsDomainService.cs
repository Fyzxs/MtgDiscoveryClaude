using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.SealedProducts.Apis;

public interface ISealedProductsDomainService
{
    Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> SealedProductsBySetCodeAsync(
        ISealedProductsBySetCodeItrEntity setCode);
}
