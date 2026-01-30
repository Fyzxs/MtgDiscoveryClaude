using System;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Itrs.User;

namespace Lib.Adapter.User.Commands.Resolvers;

internal sealed class UserInfoResolver : IUserInfoResolver
{
    public UserInfoExtEntity Resolve(OpResponse<UserInfoExtEntity> input, IUserInfoItrEntity context)
    {
        DateTime now = DateTime.UtcNow;

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
