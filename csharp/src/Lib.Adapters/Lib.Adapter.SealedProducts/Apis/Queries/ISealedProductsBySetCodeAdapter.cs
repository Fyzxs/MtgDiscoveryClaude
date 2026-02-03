using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.SealedProducts.Apis.Entities;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.SealedProducts.Apis.Queries;

/// <summary>
/// Single-operation adapter interface for retrieving sealed products by set code.
/// </summary>
internal interface ISealedProductsBySetCodeAdapter
{
    /// <summary>
    /// Executes the sealed products lookup by set code.
    /// </summary>
    /// <param name="input">The query parameters containing the set code.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Operation response containing the sealed products for the set.</returns>
    Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> Execute(
        ISealedProductsBySetCodeXfrEntity input,
        CancellationToken cancellationToken);
}
