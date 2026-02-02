using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Adapter.SealedProducts.Queries;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.SealedProducts.Apis;

/// <summary>
/// Composite service that coordinates all sealed products adapter operations.
/// Follows the 3-tier pattern: AdapterService → QueryAdapter → Single-Operation Adapters
/// </summary>
public sealed class SealedProductsAdapterService : ISealedProductsAdapterService
{
    private readonly ISealedProductsQueryAdapter _queryAdapter;

    public SealedProductsAdapterService(ILogger logger) : this(new SealedProductsQueryAdapter(logger))
    { }

    private SealedProductsAdapterService(ISealedProductsQueryAdapter queryAdapter) => _queryAdapter = queryAdapter;

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> GetBySetCodeAsync(
        ISealedProductsBySetCodeXfrEntity query,
        CancellationToken cancellationToken)
        => await _queryAdapter.GetBySetCodeAsync(query, cancellationToken).ConfigureAwait(false);
}
