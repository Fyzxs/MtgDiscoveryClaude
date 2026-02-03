using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Adapter.UserSealedProducts.Commands;
using Lib.Adapter.UserSealedProducts.Queries;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSealedProducts.Apis;

/// <summary>
/// Composite service that coordinates all user sealed products adapter operations.
/// Implements the passthrough pattern, delegating to specialized query and command adapters.
///
/// Architecture: AdapterService → QueryAdapter/CommandAdapter → Single-Operation Adapters
///
/// This service provides a unified API for all user sealed products operations while internally
/// organizing the implementation across specialized adapters for queries and commands.
/// </summary>
public sealed class UserSealedProductsAdapterService : IUserSealedProductsAdapterService
{
    private readonly IUserSealedProductsCommandAdapter _commandAdapter;
    private readonly IUserSealedProductsQueryAdapter _queryAdapter;

    public UserSealedProductsAdapterService(ILogger logger) : this(
        new UserSealedProductsCommandAdapter(logger),
        new UserSealedProductsQueryAdapter(logger))
    { }

    private UserSealedProductsAdapterService(
        IUserSealedProductsCommandAdapter commandAdapter,
        IUserSealedProductsQueryAdapter queryAdapter)
    {
        _commandAdapter = commandAdapter;
        _queryAdapter = queryAdapter;
    }

    public async Task<IOperationResponse<UserSealedProductExtEntity>> AddUserSealedProductAsync(IUserSealedProductXfrEntity input) => await _commandAdapter.AddUserSealedProductAsync(input).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserSealedProductExtEntity>>> UserSealedProductsByUserIdAsync(string collectionId) => await _queryAdapter.UserSealedProductsByUserIdAsync(collectionId).ConfigureAwait(false);
}
