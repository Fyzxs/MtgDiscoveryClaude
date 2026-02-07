using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserSealedProducts.Apis;

public interface IUserSealedProductsQueryAggregatorService
{
    Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> UserSealedProductsByUserIdAsync(IUserIdItrEntity input, CancellationToken cancellationToken);
}
