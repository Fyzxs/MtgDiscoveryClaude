using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ISealedProductsEntryService
{
    Task<IOperationResponse<List<SealedProductOutEntity>>> SealedProductsBySetCodeAsync(
        ISealedProductsBySetCodeArgEntity args,
        CancellationToken cancellationToken);
}
