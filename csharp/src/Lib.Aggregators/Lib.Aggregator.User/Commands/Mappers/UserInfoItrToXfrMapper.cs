using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.User.Apis.Entities;
using Lib.Aggregator.User.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.User;

namespace Lib.Aggregator.User.Commands.Mappers;

/// <summary>
/// Maps IUserInfoItrEntity to IUserInfoXfrEntity for adapter consumption.
/// </summary>
internal sealed class UserInfoItrToXfrMapper : IUserInfoItrToXfrMapper
{
    public Task<IUserInfoXfrEntity> Map([NotNull] IUserInfoItrEntity source)
    {
        IUserInfoXfrEntity result = new UserInfoXfrEntity
        {
            UserId = source.UserId,
            UserSourceId = source.UserSourceId,
            UserNickname = source.UserNickname,
            Email = source.Email
        };

        return Task.FromResult(result);
    }
}
