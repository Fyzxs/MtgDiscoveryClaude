using System.Threading.Tasks;
using Lib.Aggregator.User.Apis;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.User.Commands;

internal sealed class UserCommandAggregator : IUserCommandAggregatorService
{
    private readonly IRegisterUserAggregatorService _registerUserOperations;

    public UserCommandAggregator(ILogger logger) : this(new RegisterUserAggregatorService(logger))
    { }

    private UserCommandAggregator(IRegisterUserAggregatorService registerUserOperations) => _registerUserOperations = registerUserOperations;

    public async Task<IOperationResponse<IUserSyncOufEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo) => await _registerUserOperations.Execute(userInfo);
}
