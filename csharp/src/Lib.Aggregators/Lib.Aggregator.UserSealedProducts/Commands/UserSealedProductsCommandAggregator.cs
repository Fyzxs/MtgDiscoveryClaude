using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserSealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSealedProducts.Commands;

internal sealed class UserSealedProductsCommandAggregator : IUserSealedProductsCommandAggregatorService
{
    private readonly IAddUserSealedProductAggregatorService _addUserSealedProductOperations;

    public UserSealedProductsCommandAggregator(ILogger logger) : this(
        new AddUserSealedProductAggregatorService(logger))
    { }

    private UserSealedProductsCommandAggregator(
        IAddUserSealedProductAggregatorService addUserSealedProductOperations) => _addUserSealedProductOperations = addUserSealedProductOperations;

    public async Task<IOperationResponse<List<ISealedProductOufEntity>>> AddUserSealedProductAsync(IAddUserSealedProductItrEntity input, CancellationToken cancellationToken) => await _addUserSealedProductOperations.Execute(input, cancellationToken).ConfigureAwait(false);
}
