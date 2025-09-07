using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Data.Apis;

public interface IUserDataService
{
    Task<IOperationResponse<IUserInfoItrEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo);
}
