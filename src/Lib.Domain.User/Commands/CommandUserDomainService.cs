using System.Threading.Tasks;
using Lib.Aggregator.User.Apis;
using Lib.Domain.User.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.User.Commands;

internal sealed class CommandUserDomainService : IUserDomainService
{
    private readonly IUserAggregatorService _userAggregatorService;

    public CommandUserDomainService(ILogger logger) : this(new UserAggregatorService(logger))
    { }

    private CommandUserDomainService(IUserAggregatorService userAggregatorService) => _userAggregatorService = userAggregatorService;

    public async Task<IOperationResponse<IUserInfoOufEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo) => await _userAggregatorService.RegisterUserAsync(userInfo).ConfigureAwait(false);
}
