using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserSealedProducts.Queries;

internal interface IUserSealedProductsByUserIdDomainService
{
    Task<IOperationResponse<IEnumerable<IUserSealedProductOufEntity>>> Execute(IUserIdItrEntity input, CancellationToken cancellationToken);
}
