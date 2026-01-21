using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.UserSealedProducts.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSealedProducts.Queries;

internal sealed class UserSealedProductsByUserIdAggregatorService : IUserSealedProductsByUserIdAggregatorService
{
    private readonly IUserSealedProductsByUserIdAdapter _adapter;

    public UserSealedProductsByUserIdAggregatorService(ILogger logger) : this(
        new UserSealedProductsByUserIdAdapter(logger))
    { }

    private UserSealedProductsByUserIdAggregatorService(IUserSealedProductsByUserIdAdapter adapter) =>
        _adapter = adapter;

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> Execute(IUserIdItrEntity input)
    {
        UserSealedProductsByUserIdXfrArgs xfrArgs = new()
        {
            UserId = input.UserId
        };

        return await _adapter.Execute(xfrArgs).ConfigureAwait(false);
    }
}
