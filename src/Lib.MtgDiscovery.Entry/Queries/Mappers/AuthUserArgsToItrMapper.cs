using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class AuthUserArgsToItrMapper : IAuthUserArgsToItrMapper
{
    public Task<IUserInfoItrEntity> Map(IAuthUserArgEntity args)
    {
        return Task.FromResult<IUserInfoItrEntity>(new UserInfoItrEntity
        {
            UserId = args.UserId,
            UserSourceId = args.SourceId,
            UserNickname = args.DisplayName
        });
    }
}
