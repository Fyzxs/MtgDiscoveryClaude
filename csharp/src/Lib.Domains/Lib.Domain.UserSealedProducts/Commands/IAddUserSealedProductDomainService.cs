using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserSealedProducts.Commands;

internal interface IAddUserSealedProductDomainService
{
    Task<IOperationResponse<List<ISealedProductOufEntity>>> Execute(IAddUserSealedProductItrEntity input, CancellationToken cancellationToken);
}
