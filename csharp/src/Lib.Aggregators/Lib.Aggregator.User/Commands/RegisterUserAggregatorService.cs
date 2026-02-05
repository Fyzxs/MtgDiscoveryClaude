using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Adapter.User.Apis;
using Lib.Adapter.User.Apis.Entities;
using Lib.Aggregator.User.Commands.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.User.Commands;

internal sealed class RegisterUserAggregatorService : IRegisterUserAggregatorService
{
    private readonly IUserAdapterService _userAdapterService;
    private readonly IUserInfoItrToXfrMapper _itrToXfrMapper;
    private readonly IUserInfoExtToSyncOufMapper _extToOufMapper;

    public RegisterUserAggregatorService(ILogger logger) : this(
        new UserAdapterService(logger),
        new UserInfoItrToXfrMapper(),
        new UserInfoExtToSyncOufMapper())
    { }

    private RegisterUserAggregatorService(
        IUserAdapterService userAdapterService,
        IUserInfoItrToXfrMapper itrToXfrMapper,
        IUserInfoExtToSyncOufMapper extToOufMapper)
    {
        _userAdapterService = userAdapterService;
        _itrToXfrMapper = itrToXfrMapper;
        _extToOufMapper = extToOufMapper;
    }

    public async Task<IOperationResponse<IUserSyncOufEntity>> Execute([NotNull] IUserInfoItrEntity input, CancellationToken cancellationToken)
    {
        IUserInfoXfrEntity xfrEntity = await _itrToXfrMapper.Map(input).ConfigureAwait(false);

        IOperationResponse<UserInfoExtEntity> response = await _userAdapterService
            .RegisterUserAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserSyncOufEntity>(response.OuterException);
        }

        IUserSyncOufEntity oufEntity = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);

        return new SuccessOperationResponse<IUserSyncOufEntity>(oufEntity);
    }
}
