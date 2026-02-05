using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.User.Apis.Entities;
using Lib.Adapter.User.Commands.Mappers;
using Lib.Adapter.User.Commands.Resolvers;
using Lib.Adapter.User.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.User.Commands;

/// <summary>
/// Registers or syncs user information in Cosmos DB storage.
/// Returns UserInfoExtEntity directly; aggregator determines isFirstLogin from timestamps.
/// </summary>
internal sealed class RegisterUserAdapter : IRegisterUserAdapter
{
    private readonly UserInfoScribe _userInfoScribe;
    private readonly UserInfoGopher _userInfoGopher;
    private readonly IUserInfoXfrToReadPointMapper _readPointMapper;
    private readonly IUserInfoResolver _userInfoResolver;

    public RegisterUserAdapter(ILogger logger) : this(
        new UserInfoScribe(logger),
        new UserInfoGopher(logger),
        new UserInfoXfrToReadPointMapper(),
        new UserInfoResolver())
    { }

    private RegisterUserAdapter(
        UserInfoScribe userInfoScribe,
        UserInfoGopher userInfoGopher,
        IUserInfoXfrToReadPointMapper readPointMapper,
        IUserInfoResolver userInfoResolver)
    {
        _userInfoScribe = userInfoScribe;
        _userInfoGopher = userInfoGopher;
        _readPointMapper = readPointMapper;
        _userInfoResolver = userInfoResolver;
    }

    public async Task<IOperationResponse<UserInfoExtEntity>> Execute([NotNull] IUserInfoXfrEntity input, CancellationToken cancellationToken)
    {
        ReadPointItem readItem = await _readPointMapper.Map(input).ConfigureAwait(false);

        OpResponse<UserInfoExtEntity> existingUserResponse = await _userInfoGopher
            .ReadAsync<UserInfoExtEntity>(readItem, cancellationToken)
            .ConfigureAwait(false);

        UserInfoExtEntity resolvedUser = _userInfoResolver.Resolve(existingUserResponse, input);

        OpResponse<UserInfoExtEntity> upsertResponse = await _userInfoScribe
            .UpsertAsync(resolvedUser, cancellationToken)
            .ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserInfoExtEntity>(
                new UserAdapterException($"Failed to upsert user {input.UserId}: {upsertResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<UserInfoExtEntity>(upsertResponse.Value!);
    }
}
