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
    private readonly ISealedProductsBySetCodeDomainService _sealedProductsBySetCodeDomainService;

    public SealedProductsDomainService(ILogger logger) : this(new SealedProductsBySetCodeDomainService(logger))
    { }

    private SealedProductsDomainService(ISealedProductsBySetCodeDomainService sealedProductsBySetCodeDomainService) => _sealedProductsBySetCodeDomainService = sealedProductsBySetCodeDomainService;

    public Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> SealedProductsBySetCodeAsync(
        ISealedProductsBySetCodeItrEntity setCode,
        CancellationToken cancellationToken) => _sealedProductsBySetCodeDomainService.Execute(setCode, cancellationToken);
}
