using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface IUserEntryService
{
    Task<IOperationResponse<IUserInfoItrEntity>> RegisterUserAsync(IAuthUserArgEntity authUser);
}
