using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSealedProducts.Queries;

internal interface IUserSealedProductsByUserIdAdapter
{
    Task<IOperationResponse<IEnumerable<UserSealedProductExtEntity>>> Execute(string collectionId, CancellationToken cancellationToken);
}
