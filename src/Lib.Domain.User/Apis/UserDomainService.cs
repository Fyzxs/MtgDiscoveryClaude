using System.Threading.Tasks;
using Lib.Aggregator.User.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.User.Apis;

public sealed class UserDomainService : IUserDomainService
{
    private readonly IUserAggregatorService _userAggregatorService;

    public UserDomainService(ILogger logger) : this(new UserAggregatorService(logger))
    { }

    private UserDomainService(IUserAggregatorService userAggregatorService) => _userAggregatorService = userAggregatorService;

    public async Task<IOperationResponse<IUserInfoItrEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo) => await _userAggregatorService.RegisterUserAsync(userInfo).ConfigureAwait(false);
}
