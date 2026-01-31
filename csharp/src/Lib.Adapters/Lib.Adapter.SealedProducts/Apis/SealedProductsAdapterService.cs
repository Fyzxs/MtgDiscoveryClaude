using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Adapter.SealedProducts.Apis.Queries;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.SealedProducts.Apis;

public sealed class SealedProductsAdapterService : ISealedProductsAdapterService
{
    private readonly ISealedProductsBySetCodeAdapter _sealedProductsBySetCodeAdapter;

    public SealedProductsAdapterService(ILogger logger) : this(new SealedProductsBySetCodeAdapter(logger))
    { }

    private SealedProductsAdapterService(ISealedProductsBySetCodeAdapter sealedProductsBySetCodeAdapter) => _sealedProductsBySetCodeAdapter = sealedProductsBySetCodeAdapter;

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> GetBySetCodeAsync(
        ISealedProductsBySetCodeXfrEntity setCode,
        CancellationToken cancellationToken) => await _sealedProductsBySetCodeAdapter.Execute(setCode).ConfigureAwait(false);
}
