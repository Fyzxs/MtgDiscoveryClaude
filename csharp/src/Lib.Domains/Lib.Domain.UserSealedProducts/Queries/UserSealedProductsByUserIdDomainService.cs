using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserSealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSealedProducts.Queries;

/// <summary>
/// Single-method service for retrieving all sealed products for a specific user.
/// Delegates to aggregator layer for data retrieval.
/// Future: Could add business rules such as filtering by category or sorting by set release date.
/// </summary>
internal sealed class UserSealedProductsByUserIdDomainService : IUserSealedProductsByUserIdDomainService
{
    private readonly IUserSealedProductsAggregatorService _aggregator;

    public UserSealedProductsByUserIdDomainService(ILogger logger) : this(
        new UserSealedProductsAggregatorService(logger))
    { }

    private UserSealedProductsByUserIdDomainService(IUserSealedProductsAggregatorService aggregator) => _aggregator = aggregator;

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> Execute(IUserIdItrEntity input) => await _aggregator.UserSealedProductsByUserIdAsync(input).ConfigureAwait(false);
}
