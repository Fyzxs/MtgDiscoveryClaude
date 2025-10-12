using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.User.Apis;

public interface IUserDomainService
{
    Task<IOperationResponse<IUserInfoOufEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo);
}
