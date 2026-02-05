using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.User;

internal interface IRegisterUserEntryService
{
    Task<IOperationResponse<UserSyncOutEntity>> Execute(IAuthUserArgEntity input, CancellationToken cancellationToken);
}
