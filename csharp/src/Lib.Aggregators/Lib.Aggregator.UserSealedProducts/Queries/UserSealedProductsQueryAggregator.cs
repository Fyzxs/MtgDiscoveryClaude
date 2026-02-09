using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserSealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSealedProducts.Queries;

internal sealed class UserSealedProductsQueryAggregator : IUserSealedProductsQueryAggregatorService
{
    private readonly IUserSealedProductsByUserIdAggregatorService _userSealedProductsByUserIdOperations;

    public UserSealedProductsQueryAggregator(ILogger logger) : this(
        new UserSealedProductsByUserIdAggregatorService(logger))
    { }

    private UserSealedProductsQueryAggregator(
        IUserSealedProductsByUserIdAggregatorService userSealedProductsByUserIdOperations) => _userSealedProductsByUserIdOperations = userSealedProductsByUserIdOperations;

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductOufEntity>>> UserSealedProductsByUserIdAsync(IUserIdItrEntity input, CancellationToken cancellationToken) => await _userSealedProductsByUserIdOperations.Execute(input, cancellationToken).ConfigureAwait(false);
}
