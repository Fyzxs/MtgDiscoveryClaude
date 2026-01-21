using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSealedProducts.Apis;

public sealed class UserSealedProductsByUserIdAggregator : IUserSealedProductsByUserIdAggregator
{
    private readonly Queries.UserSealedProductsByUserIdAggregator _aggregator;

    public UserSealedProductsByUserIdAggregator(ILogger logger) : this(
        new Queries.UserSealedProductsByUserIdAggregator(logger))
    { }

    private UserSealedProductsByUserIdAggregator(Queries.UserSealedProductsByUserIdAggregator aggregator) =>
        _aggregator = aggregator;

    public Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> Execute(IUserIdItrEntity input) =>
        _aggregator.Execute(input);
}
