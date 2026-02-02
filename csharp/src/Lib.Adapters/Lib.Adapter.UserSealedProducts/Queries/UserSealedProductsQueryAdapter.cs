using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSealedProducts.Apis;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSealedProducts.Queries;

internal sealed class UserSealedProductsQueryAdapter : IUserSealedProductsQueryAdapter
{
    private readonly IUserSealedProductsByUserIdAdapter _userSealedProductsByUserIdAdapter;

    public UserSealedProductsQueryAdapter(ILogger logger) : this(
        new UserSealedProductsByUserIdAdapter(logger))
    { }

    private UserSealedProductsQueryAdapter(IUserSealedProductsByUserIdAdapter userSealedProductsByUserIdAdapter) => _userSealedProductsByUserIdAdapter = userSealedProductsByUserIdAdapter;

    public async Task<IOperationResponse<IEnumerable<UserSealedProductExtEntity>>> UserSealedProductsByUserIdAsync(string userId) => await _userSealedProductsByUserIdAdapter.Execute(userId).ConfigureAwait(false);
}
