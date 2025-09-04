using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface IUserEntryService
{
    Task<IOperationResponse<IUserRegistrationItrEntity>> RegisterUserAsync(IAuthUserArgEntity authUser);
}