using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.UserInfo.Values;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class AuthUserArgsToItrMapper : IAuthUserArgsToItrMapper
{
    public async Task<IUserInfoItrEntity> Map(IAuthUserArgEntity args)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        return new UserInfoItrEntity
        {
            UserId = new UserId(args.UserId),
            UserSourceId = new UserSourceId(args.SourceId),
            UserNickname = new UserNickname(args.DisplayName)
        };
    }
}