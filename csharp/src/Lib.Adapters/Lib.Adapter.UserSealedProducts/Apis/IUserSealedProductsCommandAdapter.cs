using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSealedProducts.Apis;

public interface IUserSealedProductsCommandAdapter
{
    Task<IOperationResponse<UserSealedProductExtEntity>> AddUserSealedProductAsync(IUserSealedProductXfrEntity input, CancellationToken cancellationToken);
}
