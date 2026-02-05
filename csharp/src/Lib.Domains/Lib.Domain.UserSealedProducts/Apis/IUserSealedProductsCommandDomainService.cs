using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserSealedProducts.Apis;

public interface IUserSealedProductsCommandDomainService
{
    Task<IOperationResponse<List<ISealedProductOufEntity>>> AddUserSealedProductAsync(IAddUserSealedProductItrEntity input, CancellationToken cancellationToken);
}
