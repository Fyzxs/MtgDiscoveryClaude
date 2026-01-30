using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class UserSyncOufToOutMapper : IUserSyncOufToOutMapper
{
    public Task<UserSyncOutEntity> Map(IUserSyncOufEntity userSync)
    {
        UserSyncOutEntity result = new()
        {
            UserId = userSync.UserId,
            DisplayName = userSync.DisplayName,
            Email = userSync.Email,
            CreatedAt = userSync.CreatedAt,
            LastLoginAt = userSync.LastLoginAt,
            IsFirstLogin = userSync.IsFirstLogin
        };

        return Task.FromResult(result);
    }
}
