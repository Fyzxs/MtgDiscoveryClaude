using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.User.Commands;

internal interface IRegisterUserAggregatorService
{
    Task<IOperationResponse<IUserSyncOufEntity>> Execute(IUserInfoItrEntity input, CancellationToken cancellationToken);
}
