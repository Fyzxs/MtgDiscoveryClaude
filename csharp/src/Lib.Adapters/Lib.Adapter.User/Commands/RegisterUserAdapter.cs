using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.User.Commands.Mappers;
using Lib.Adapter.User.Commands.Resolvers;
using Lib.Adapter.User.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.User.Commands;

/// <summary>
/// Registers or syncs user information in Cosmos DB storage.
/// Returns isFirstLogin flag based on whether user already existed.
/// </summary>
internal sealed class RegisterUserAdapter : IRegisterUserAdapter
{
    private readonly UserInfoScribe _userInfoScribe;
    private readonly UserInfoGopher _userInfoGopher;
    private readonly IUserInfoItrToReadPointMapper _readPointMapper;
    private readonly IUserInfoResolver _userInfoResolver;
    private readonly IUserInfoExtToSyncOufMapper _syncOufMapper;

    public RegisterUserAdapter(ILogger logger) : this(
        new UserInfoScribe(logger),
        new UserInfoGopher(logger),
        new UserInfoItrToReadPointMapper(),
        new UserInfoResolver(),
        new UserInfoExtToSyncOufMapper())
    { }

    private RegisterUserAdapter(
        UserInfoScribe userInfoScribe,
        UserInfoGopher userInfoGopher,
        IUserInfoItrToReadPointMapper readPointMapper,
        IUserInfoResolver userInfoResolver,
        IUserInfoExtToSyncOufMapper syncOufMapper)
    {
        _userInfoScribe = userInfoScribe;
        _userInfoGopher = userInfoGopher;
        _readPointMapper = readPointMapper;
        _userInfoResolver = userInfoResolver;
        _syncOufMapper = syncOufMapper;
    }

    public async Task<IOperationResponse<IUserSyncOufEntity>> Execute([NotNull] IUserInfoItrEntity input, CancellationToken cancellationToken)
    {
        ReadPointItem readItem = await _readPointMapper.Map(input).ConfigureAwait(false);

        OpResponse<UserInfoExtEntity> existingUserResponse = await _userInfoGopher
            .ReadAsync<UserInfoExtEntity>(readItem, cancellationToken)
            .ConfigureAwait(false);

        bool isFirstLogin = existingUserResponse.IsNotSuccessful() ||
                            existingUserResponse.Value is null;

        UserInfoExtEntity resolvedUser = _userInfoResolver.Resolve(existingUserResponse, input);

        OpResponse<UserInfoExtEntity> upsertResponse = await _userInfoScribe
            .UpsertAsync(resolvedUser, cancellationToken)
            .ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IUserSyncOufEntity>(
                new UserAdapterException($"Failed to upsert user {input.UserId}: {upsertResponse.StatusCode}"));
        }

        IUserSyncOufEntity syncResult = await _syncOufMapper
            .Map(upsertResponse.Value!, isFirstLogin)
            .ConfigureAwait(false);

        return new SuccessOperationResponse<IUserSyncOufEntity>(syncResult);
    }
}
