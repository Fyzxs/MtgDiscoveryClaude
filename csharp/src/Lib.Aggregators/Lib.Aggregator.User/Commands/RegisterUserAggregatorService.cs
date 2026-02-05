using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.User.Apis;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.User.Commands;

internal sealed class RegisterUserAggregatorService : IRegisterUserAggregatorService
{
    private readonly IUserAdapterService _userAdapterService;

    public RegisterUserAggregatorService(ILogger logger) : this(new UserAdapterService(logger))
    { }

    private RegisterUserAggregatorService(IUserAdapterService userAdapterService) => _userAdapterService = userAdapterService;

    public async Task<IOperationResponse<IUserSyncOufEntity>> Execute([NotNull] IUserInfoItrEntity input, CancellationToken cancellationToken)
    {
        IOperationResponse<IUserSyncOufEntity> response = await _userAdapterService
            .RegisterUserAsync(input, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserSyncOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<IUserSyncOufEntity>(response.ResponseData);
    }
}
