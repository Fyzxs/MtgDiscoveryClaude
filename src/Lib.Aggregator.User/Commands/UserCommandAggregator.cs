using System.Threading.Tasks;
using Lib.Aggregator.User.Apis;
using Lib.Aggregator.User.Commands.RegisterUser;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.User.Commands;

internal sealed class UserCommandAggregator : IUserCommandAggregatorService
{
    private readonly IRegisterUserAggregatorService _registerUserOperations;

    public UserCommandAggregator(ILogger logger) : this(new RegisterUserAggregatorService(logger))
    { }

    private UserCommandAggregator(IRegisterUserAggregatorService registerUserOperations) => _registerUserOperations = registerUserOperations;

    public Task<IOperationResponse<IUserInfoOufEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo) => _registerUserOperations.Execute(userInfo);
}
