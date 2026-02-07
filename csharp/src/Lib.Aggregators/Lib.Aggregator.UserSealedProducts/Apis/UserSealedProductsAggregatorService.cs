using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserSealedProducts.Commands;
using Lib.Aggregator.UserSealedProducts.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSealedProducts.Apis;

public sealed class UserSealedProductsAggregatorService : IUserSealedProductsAggregatorService
{
    private readonly IUserSealedProductsCommandAggregatorService _commandOperations;
    private readonly IUserSealedProductsQueryAggregatorService _queryOperations;

    public UserSealedProductsAggregatorService(ILogger logger) : this(
        new UserSealedProductsCommandAggregator(logger),
        new UserSealedProductsQueryAggregator(logger))
    { }

    private UserSealedProductsAggregatorService(
        IUserSealedProductsCommandAggregatorService commandOperations,
        IUserSealedProductsQueryAggregatorService queryOperations)
    {
        _commandOperations = commandOperations;
        _queryOperations = queryOperations;
    }

    public async Task<IOperationResponse<List<ISealedProductOufEntity>>> AddUserSealedProductAsync(IAddUserSealedProductItrEntity input, CancellationToken cancellationToken) => await _commandOperations.AddUserSealedProductAsync(input, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> UserSealedProductsByUserIdAsync(IUserIdItrEntity input, CancellationToken cancellationToken) => await _queryOperations.UserSealedProductsByUserIdAsync(input, cancellationToken).ConfigureAwait(false);
}
