using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.User.Apis;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.User.Commands;

internal sealed class RegisterUserDomain : IRegisterUserDomain
{
    private readonly IUserAggregatorService _userAggregatorService;

    public RegisterUserDomain(ILogger logger) : this(new UserAggregatorService(logger))
    { }

    private RegisterUserDomain(IUserAggregatorService userAggregatorService) => _userAggregatorService = userAggregatorService;

    public async Task<IOperationResponse<IUserSyncOufEntity>> Execute(IUserInfoItrEntity input, CancellationToken cancellationToken) => await _userAggregatorService.RegisterUserAsync(input, cancellationToken).ConfigureAwait(false);
}
