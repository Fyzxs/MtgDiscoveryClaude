using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Shared.DataModels.Entities.Oufs.User;

namespace Lib.Adapter.User.Commands.Mappers;

internal sealed class UserInfoExtToSyncOufMapper : IUserInfoExtToSyncOufMapper
{
    public Task<IUserSyncOufEntity> Map(UserInfoExtEntity source, bool isFirstLogin)
    {
        IUserSyncOufEntity result = new UserSyncOufEntity
        {
            UserId = source.UserId,
            DisplayName = source.DisplayName,
            Email = source.Email,
            CreatedAt = source.CreatedAt,
            LastLoginAt = source.LastLoginAt,
            IsFirstLogin = isFirstLogin
        };

        return Task.FromResult(result);
    }
}
