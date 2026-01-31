using Lib.Shared.DataModels.Entities.Oufs.User;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class UserInfoOufEntity : IUserInfoOufEntity
{
    public string UserId { get; init; }
    public string UserSourceId { get; init; }
    public string UserNickname { get; init; }
}
