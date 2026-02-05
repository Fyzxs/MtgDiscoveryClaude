using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserSealedProducts.Commands;

internal interface IAddUserSealedProductAggregatorService
{
    Task<IOperationResponse<List<ISealedProductOufEntity>>> Execute(IAddUserSealedProductItrEntity input, CancellationToken cancellationToken);
}
