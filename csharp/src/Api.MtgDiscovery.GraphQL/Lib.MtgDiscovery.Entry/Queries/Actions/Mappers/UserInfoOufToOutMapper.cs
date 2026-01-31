using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class UserInfoOufToOutMapper : IUserInfoOufToOutMapper
{
    public Task<UserRegistrationOutEntity> Map(IUserInfoOufEntity userInfo)
    {
        UserRegistrationOutEntity result = new()
        {
            UserId = userInfo.UserId,
            DisplayName = userInfo.UserNickname
        };

        return Task.FromResult(result);
    }
}
