using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.SealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.SealedProducts.Queries;

internal sealed class SealedProductsBySetCodeDomain : ISealedProductsBySetCodeDomain
{
    private readonly ISealedProductsAggregatorService _aggregatorService;

    public SealedProductsBySetCodeDomain(ILogger logger) : this(new SealedProductsAggregatorService(logger))
    { }

    private SealedProductsBySetCodeDomain(ISealedProductsAggregatorService aggregatorService) => _aggregatorService = aggregatorService;

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> Execute(
        ISealedProductsBySetCodeItrEntity input,
        CancellationToken cancellationToken) => await _aggregatorService.SealedProductsBySetCodeAsync(input, cancellationToken).ConfigureAwait(false);
}
