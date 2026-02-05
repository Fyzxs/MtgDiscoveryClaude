using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSealedProducts.Commands;

internal interface IAddUserSealedProductAdapter
{
    Task<IOperationResponse<UserSealedProductExtEntity>> Execute(IUserSealedProductXfrEntity input, CancellationToken cancellationToken);
}
