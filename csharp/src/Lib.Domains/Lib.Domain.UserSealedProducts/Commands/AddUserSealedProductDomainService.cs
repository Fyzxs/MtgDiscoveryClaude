using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserSealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSealedProducts.Commands;

internal sealed class AddUserSealedProductDomainService : IAddUserSealedProductDomainService
{
    private readonly IUserSealedProductsAggregatorService _aggregator;

    public AddUserSealedProductDomainService(ILogger logger) : this(
        new UserSealedProductsAggregatorService(logger))
    { }

    private AddUserSealedProductDomainService(IUserSealedProductsAggregatorService aggregator) => _aggregator = aggregator;

    public async Task<IOperationResponse<List<ISealedProductOufEntity>>> Execute(IAddUserSealedProductItrEntity input, CancellationToken cancellationToken) => await _aggregator.AddUserSealedProductAsync(input, cancellationToken).ConfigureAwait(false);
}
