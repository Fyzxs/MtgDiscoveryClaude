using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.SealedProducts.Queries;

internal interface ISealedProductsBySetCodeDomainService
{
    Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> Execute(
        ISealedProductsBySetCodeItrEntity input,
        CancellationToken cancellationToken);
}
