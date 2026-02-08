using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserSealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSealedProducts.Queries;

internal sealed class UserSealedProductsByUserIdDomain : IUserSealedProductsByUserIdDomain
{
    private readonly IUserSealedProductsAggregatorService _aggregator;

    public UserSealedProductsByUserIdDomain(ILogger logger) : this(
        new UserSealedProductsAggregatorService(logger))
    { }

    private UserSealedProductsByUserIdDomain(IUserSealedProductsAggregatorService aggregator) => _aggregator = aggregator;

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductOufEntity>>> Execute(IUserIdItrEntity input, CancellationToken cancellationToken) => await _aggregator.UserSealedProductsByUserIdAsync(input, cancellationToken).ConfigureAwait(false);
}
