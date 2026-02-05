using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSealedProducts.Apis;

public interface IUserSealedProductsQueryAdapter
{
    Task<IOperationResponse<IEnumerable<UserSealedProductExtEntity>>> UserSealedProductsByUserIdAsync(string collectionId, CancellationToken cancellationToken);
}
