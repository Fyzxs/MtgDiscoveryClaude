using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.SealedProducts.Apis.Queries;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.SealedProducts.Apis;

public sealed class SealedProductsAggregatorService : ISealedProductsAggregatorService
{
    private readonly ISealedProductsBySetCodeAggregator _sealedProductsBySetCodeAggregator;

    public SealedProductsAggregatorService(ILogger logger) : this(new SealedProductsBySetCodeAggregator(logger))
    { }

    private SealedProductsAggregatorService(ISealedProductsBySetCodeAggregator sealedProductsBySetCodeAggregator) => _sealedProductsBySetCodeAggregator = sealedProductsBySetCodeAggregator;

    public Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> SealedProductsBySetCodeAsync(
        ISealedProductsBySetCodeItrEntity setCode,
        CancellationToken cancellationToken) => _sealedProductsBySetCodeAggregator.Execute(setCode, cancellationToken);
}
