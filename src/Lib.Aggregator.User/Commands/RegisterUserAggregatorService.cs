using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.User.Apis;
using Lib.Aggregator.User.Commands.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.User.Commands;

internal sealed class RegisterUserAggregatorService : IRegisterUserAggregatorService
{
    private readonly IUserAdapterService _userAdapterService;
    private readonly IUserInfoExtToItrEntityMapper _userInfoMapper;

    public RegisterUserAggregatorService(ILogger logger) : this(
        new UserAdapterService(logger),
        new UserInfoExtToItrEntityMapper())
    { }

    private RegisterUserAggregatorService(
        IUserAdapterService userAdapterService,
        IUserInfoExtToItrEntityMapper userInfoMapper)
    {
        _userAdapterService = userAdapterService;
        _userInfoMapper = userInfoMapper;
    }

    public async Task<IOperationResponse<IUserInfoOufEntity>> Execute([NotNull] IUserInfoItrEntity input)
    {
        IOperationResponse<UserInfoExtEntity> response = await _userAdapterService.RegisterUserAsync(input).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserInfoOufEntity>(response.OuterException);
        }

        IUserInfoOufEntity mappedUserInfo = await _userInfoMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserInfoOufEntity>(mappedUserInfo);
    }
}
