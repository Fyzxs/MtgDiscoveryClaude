using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Shared.DataModels.Entities.Oufs.User;

namespace Lib.Aggregator.User.Commands.Mappers;

/// <summary>
/// Maps UserInfoExtEntity to IUserSyncOufEntity.
/// Computes IsFirstLogin from timestamps (CreatedAt == LastLoginAt indicates first login).
/// </summary>
internal sealed class UserInfoExtToSyncOufMapper : IUserInfoExtToSyncOufMapper
{
    public Task<IUserSyncOufEntity> Map([NotNull] UserInfoExtEntity source)
    {
        IUserSyncOufEntity result = new UserSyncOufEntity
        {
            UserId = source.UserId,
            DisplayName = source.DisplayName,
            Email = source.Email,
            CreatedAt = source.CreatedAt,
            LastLoginAt = source.LastLoginAt,
            IsFirstLogin = source.CreatedAt == source.LastLoginAt
        };

        return Task.FromResult(result);
    }
}
