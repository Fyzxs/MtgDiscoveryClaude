using System;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Adapter.User.Apis.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Universal.Primitives;

namespace Lib.Adapter.User.Commands.Resolvers;

internal sealed class UserInfoResolver : IUserInfoResolver
{
    private readonly TimeInstant _timeInstant;

    public UserInfoResolver()
        : this(new UtcNowTimeInstant())
    { }

    private UserInfoResolver(TimeInstant timeInstant) => _timeInstant = timeInstant;

    public UserInfoExtEntity Resolve(OpResponse<UserInfoExtEntity> input, IUserInfoXfrEntity context)
    {
        DateTime now = _timeInstant;

        DateTime createdAt = input.IsSuccessful() && input.Value is not null && input.Value.CreatedAt != default
            ? input.Value.CreatedAt
            : now;

        return new UserInfoExtEntity
        {
            UserId = context.UserId,
            DisplayName = context.UserNickname,
            SourceId = context.UserSourceId,
            Email = context.Email,
            CreatedAt = createdAt,
            LastLoginAt = now
        };
    }
}
