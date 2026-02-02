using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.SealedProducts.Apis;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Adapter.SealedProducts.Apis.Queries;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.SealedProducts.Queries;

/// <summary>
/// Coordinates all sealed products query operations by delegating to specialized single-method adapters.
/// The main SealedProductsAdapterService delegates to this implementation.
/// </summary>
internal sealed class SealedProductsQueryAdapter : ISealedProductsQueryAdapter
{
    private readonly ISealedProductsBySetCodeAdapter _sealedProductsBySetCodeAdapter;

    public SealedProductsQueryAdapter(ILogger logger) : this(new SealedProductsBySetCodeAdapter(logger))
    { }

    private SealedProductsQueryAdapter(ISealedProductsBySetCodeAdapter sealedProductsBySetCodeAdapter)
    {
        _sealedProductsBySetCodeAdapter = sealedProductsBySetCodeAdapter;
    }

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> GetBySetCodeAsync(
        ISealedProductsBySetCodeXfrEntity query,
        CancellationToken cancellationToken)
        => await _sealedProductsBySetCodeAdapter.Execute(query).ConfigureAwait(false);
}
