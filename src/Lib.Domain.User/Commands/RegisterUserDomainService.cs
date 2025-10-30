using System.Threading.Tasks;
using Lib.Aggregator.User.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.User.Commands;

/// <summary>
/// Single-method service for user registration operation.
/// Delegates to aggregator layer for data operations.
/// </summary>
internal sealed class RegisterUserDomainService : IRegisterUserDomainService
{
    private readonly IUserAggregatorService _userAggregatorService;

    public RegisterUserDomainService(ILogger logger) : this(new UserAggregatorService(logger))
    { }

    private RegisterUserDomainService(IUserAggregatorService userAggregatorService) => _userAggregatorService = userAggregatorService;

    public async Task<IOperationResponse<IUserInfoOufEntity>> Execute(IUserInfoItrEntity input) => await _userAggregatorService.RegisterUserAsync(input).ConfigureAwait(false);
}
