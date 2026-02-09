using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.SealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.SealedProducts.Queries;

internal sealed class SealedProductsQueryDomainService : ISealedProductsQueryDomainService
{
    private readonly ISealedProductsBySetCodeDomain _sealedProductsBySetCode;

    public SealedProductsQueryDomainService(ILogger logger) : this(
        new SealedProductsBySetCodeDomain(logger))
    { }

    private SealedProductsQueryDomainService(
        ISealedProductsBySetCodeDomain sealedProductsBySetCode)
        => _sealedProductsBySetCode = sealedProductsBySetCode;

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> SealedProductsBySetCodeAsync(
        ISealedProductsBySetCodeItrEntity setCode,
        CancellationToken cancellationToken) => await _sealedProductsBySetCode.Execute(setCode, cancellationToken).ConfigureAwait(false);
}
