using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.User;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class UserInfoOufToOutMapper : IUserInfoOufToOutMapper
{
    public UserInfoOufToOutMapper()
    {
    }

    public async Task<UserRegistrationOutEntity> Map(IUserInfoOufEntity userInfo)
    {
        await Task.CompletedTask.ConfigureAwait(false);

        return new UserRegistrationOutEntity()
        {
            UserId = userInfo.UserId,
            DisplayName = userInfo.UserNickname
        };
    }
}
