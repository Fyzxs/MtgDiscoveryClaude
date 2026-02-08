using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.SealedProducts.Queries;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.SealedProducts.Apis;

public sealed class SealedProductsDomainService : ISealedProductsDomainService
{
    private readonly ISealedProductsQueryDomainService _sealedProductsDomainOperations;

    public SealedProductsDomainService(ILogger logger) : this(new SealedProductsQueryDomainService(logger))
    { }

    private SealedProductsDomainService(ISealedProductsQueryDomainService sealedProductsDomainOperations) => _sealedProductsDomainOperations = sealedProductsDomainOperations;

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> SealedProductsBySetCodeAsync(
        ISealedProductsBySetCodeItrEntity setCode,
        CancellationToken cancellationToken) => await _sealedProductsDomainOperations.SealedProductsBySetCodeAsync(setCode, cancellationToken).ConfigureAwait(false);
}
