using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSealedProducts.Apis;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSealedProducts.Commands;

internal sealed class UserSealedProductsCommandAdapter : IUserSealedProductsCommandAdapter
{
    private readonly IAddUserSealedProductAdapter _addUserSealedProductAdapter;

    public UserSealedProductsCommandAdapter(ILogger logger) : this(
        new AddUserSealedProductAdapter(logger))
    { }

    private UserSealedProductsCommandAdapter(IAddUserSealedProductAdapter addUserSealedProductAdapter) =>
        _addUserSealedProductAdapter = addUserSealedProductAdapter;

    public Task<IOperationResponse<UserSealedProductExtEntity>> AddUserSealedProductAsync(IUserSealedProductXfrEntity input) =>
        _addUserSealedProductAdapter.Execute(input);
}
