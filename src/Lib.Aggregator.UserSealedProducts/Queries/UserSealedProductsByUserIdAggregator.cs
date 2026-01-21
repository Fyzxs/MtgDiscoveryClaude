using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.UserSealedProducts.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSealedProducts.Queries;

internal sealed class UserSealedProductsByUserIdAggregator : IUserSealedProductsByUserIdAggregator
{
    private readonly IUserSealedProductsByUserIdAdapter _adapter;

    public UserSealedProductsByUserIdAggregator(ILogger logger) : this(
        new UserSealedProductsByUserIdAdapter(logger))
    { }

    private UserSealedProductsByUserIdAggregator(IUserSealedProductsByUserIdAdapter adapter)
    {
        _adapter = adapter;
    }

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> Execute(string input)
    {
        UserSealedProductsByUserIdXfrArgs xfrArgs = new()
        {
            UserId = input
        };

        return await _adapter.Execute(xfrArgs).ConfigureAwait(false);
    }
}
