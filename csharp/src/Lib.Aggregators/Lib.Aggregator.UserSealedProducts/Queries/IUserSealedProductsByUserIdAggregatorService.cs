using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserSealedProducts.Queries;

internal interface IUserSealedProductsByUserIdAggregatorService
{
    Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> Execute(IUserIdItrEntity input, CancellationToken cancellationToken);
}
