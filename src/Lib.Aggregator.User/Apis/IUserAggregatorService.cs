using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.User.Apis;

public interface IUserAggregatorService
{
    Task<IOperationResponse<IUserInfoOufEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo);
}
