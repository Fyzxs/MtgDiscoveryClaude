using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.User.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.User.Apis;

public sealed class UserAggregatorService : IUserAggregatorService
{
    private readonly IUserAdapterService _userAdapterService;

    public UserAggregatorService(ILogger logger) : this(new UserAdapterService(logger))
    { }

    private UserAggregatorService(IUserAdapterService userAdapterService)
    {
        _userAdapterService = userAdapterService;
    }

    public async Task<IOperationResponse<IUserInfoItrEntity>> RegisterUserAsync([NotNull] IUserInfoItrEntity userInfo)
    {
        return await _userAdapterService.RegisterUserAsync(userInfo).ConfigureAwait(false);
    }
}
