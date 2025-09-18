using System.Threading.Tasks;
using Lib.Aggregator.User.Commands;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.User.Apis;

public sealed class UserAggregatorService : IUserAggregatorService
{
    private readonly IUserAggregatorService _userAggregatorOperations;

    public UserAggregatorService(ILogger logger) : this(new CommandUserAggregatorService(logger))
    { }

    private UserAggregatorService(IUserAggregatorService userAggregatorOperations)
    {
        _userAggregatorOperations = userAggregatorOperations;
    }

    public Task<IOperationResponse<IUserInfoItrEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo)
    {
        return _userAggregatorOperations.RegisterUserAsync(userInfo);
    }
}
