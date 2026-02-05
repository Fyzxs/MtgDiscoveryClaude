using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.UserSealedProducts;

internal interface IUserSealedProductsByUserIdEntryService
{
    Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> Execute(string userId, CancellationToken cancellationToken);
}
