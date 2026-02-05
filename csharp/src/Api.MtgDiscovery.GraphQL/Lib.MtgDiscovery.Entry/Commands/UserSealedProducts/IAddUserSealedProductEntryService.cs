using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.UserSealedProducts;

internal interface IAddUserSealedProductEntryService
{
    Task<IOperationResponse<List<SealedProductOutEntity>>> Execute(IAddSealedProductToCollectionArgsEntity input, CancellationToken cancellationToken);
}
