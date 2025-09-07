using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.UserInfo.Values;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class AuthUserArgsToItrMapper : IAuthUserArgsToItrMapper
{
    public Task<IUserInfoItrEntity> Map(IAuthUserArgEntity args)
    {
        return Task.FromResult<IUserInfoItrEntity>(new UserInfoItrEntity
        {
            UserId = new UserId(args.UserId),
            UserSourceId = new UserSourceId(args.SourceId),
            UserNickname = new UserNickname(args.DisplayName)
        });
    }
}
