using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.User.Apis;
using Lib.Aggregator.User.Apis;
using Lib.Aggregator.User.Commands.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.User.Commands;

internal sealed class CommandUserAggregatorService : IUserAggregatorService
{
    private readonly IUserAdapterService _userAdapterService;
    private readonly IUserInfoExtToItrEntityMapper _userInfoMapper;

    public CommandUserAggregatorService(ILogger logger) : this(
        new UserAdapterService(logger),
        new UserInfoExtToItrEntityMapper())
    { }

    private CommandUserAggregatorService(
        IUserAdapterService userAdapterService,
        IUserInfoExtToItrEntityMapper userInfoMapper)
    {
        _userAdapterService = userAdapterService;
        _userInfoMapper = userInfoMapper;
    }

    public async Task<IOperationResponse<IUserInfoItrEntity>> RegisterUserAsync([NotNull] IUserInfoItrEntity userInfo)
    {
        IOperationResponse<UserInfoExtEntity> response = await _userAdapterService.RegisterUserAsync(userInfo).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserInfoItrEntity>(response.OuterException);
        }

        IUserInfoItrEntity mappedUserInfo = await _userInfoMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserInfoItrEntity>(mappedUserInfo);
    }
}