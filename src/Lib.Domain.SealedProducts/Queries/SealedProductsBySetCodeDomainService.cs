using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.SealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.SealedProducts.Queries;

internal sealed class SealedProductsBySetCodeDomainService : ISealedProductsBySetCodeDomainService
{
    private readonly ISealedProductsAggregatorService _aggregatorService;

    public SealedProductsBySetCodeDomainService(ILogger logger) : this(new SealedProductsAggregatorService(logger))
    { }

    private SealedProductsBySetCodeDomainService(ISealedProductsAggregatorService aggregatorService) =>
        _aggregatorService = aggregatorService;

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> Execute(
        ISealedProductsBySetCodeItrEntity input) =>
        await _aggregatorService.SealedProductsBySetCodeAsync(input).ConfigureAwait(false);
}
