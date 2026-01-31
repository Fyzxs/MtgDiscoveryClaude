using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.User.Apis;

public interface IUserCommandAggregatorService
{
    Task<IOperationResponse<IUserSyncOufEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo);
}
