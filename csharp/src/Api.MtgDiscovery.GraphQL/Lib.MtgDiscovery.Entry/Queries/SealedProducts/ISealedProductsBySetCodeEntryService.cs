using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.SealedProducts;

public interface ISealedProductsBySetCodeEntryService
{
    Task<IOperationResponse<List<SealedProductOutEntity>>> Execute(
        ISealedProductsBySetCodeArgEntity args,
        CancellationToken cancellationToken);
}
