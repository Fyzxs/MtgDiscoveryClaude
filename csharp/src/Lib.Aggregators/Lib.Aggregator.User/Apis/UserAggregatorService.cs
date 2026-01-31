using System.Threading.Tasks;
using Lib.Aggregator.User.Commands;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.User.Apis;

public sealed class UserAggregatorService : IUserAggregatorService
{
    private readonly IUserCommandAggregatorService _userAggregatorOperations;

    public UserAggregatorService(ILogger logger) : this(new UserCommandAggregator(logger))
    { }

    private UserAggregatorService(IUserCommandAggregatorService userAggregatorOperations) => _userAggregatorOperations = userAggregatorOperations;

    public async Task<IOperationResponse<IUserSyncOufEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo) => await _userAggregatorOperations.RegisterUserAsync(userInfo);
}
