using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.User.Apis;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.User.Commands;

/// <summary>
/// Single-method service for user registration/sync operation.
/// Delegates to aggregator layer for data operations.
/// Returns isFirstLogin flag to indicate new vs returning user.
/// </summary>
internal sealed class RegisterUserDomainService : IRegisterUserDomainService
{
    private readonly IUserAggregatorService _userAggregatorService;

    public RegisterUserDomainService(ILogger logger) : this(new UserAggregatorService(logger))
    { }

    private RegisterUserDomainService(IUserAggregatorService userAggregatorService) => _userAggregatorService = userAggregatorService;

    public async Task<IOperationResponse<IUserSyncOufEntity>> Execute(IUserInfoItrEntity input, CancellationToken cancellationToken) => await _userAggregatorService.RegisterUserAsync(input, cancellationToken).ConfigureAwait(false);
}
