using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Adapter.UserSealedProducts.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Cosmos.Apis.Primitives.CosmosItems.Inquisitions;
using Lib.Cosmos.Apis.Primitives.CosmosItems.PartitionKeyValues;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSealedProducts.Queries;

internal sealed class UserSealedProductsByUserIdAdapter : IUserSealedProductsByUserIdAdapter
{
    private readonly ICosmosInquisitor _userSealedProductsInquisitor;

    public UserSealedProductsByUserIdAdapter(ILogger logger) : this(
        new UserSealedProductsInquisitor(logger))
    { }

    private UserSealedProductsByUserIdAdapter(
        ICosmosInquisitor userSealedProductsInquisitor) =>
        _userSealedProductsInquisitor = userSealedProductsInquisitor;

    public async Task<IOperationResponse<IEnumerable<UserSealedProductExtEntity>>> Execute(string userId)
    {
        QueryInquisitionItem query = new QueryInquisitionItem
        {
            PartitionKeyValue = new ProvidedPartitionKeyValue(userId),
            Query = "SELECT * FROM c"
        };

        OpResponse<IEnumerable<UserSealedProductExtEntity>> response =
            await _userSealedProductsInquisitor.InquisiteAsync<UserSealedProductExtEntity>(query)
                .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserSealedProductExtEntity>>(
                new UserSealedProductsAdapterException(
                    $"Failed to query user sealed products: {response.StatusCode}"));
        }

        return new SuccessOperationResponse<IEnumerable<UserSealedProductExtEntity>>(response.Value);
    }
}
