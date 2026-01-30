using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Entities.User;

internal sealed class UserIdArgEntity : IUserIdArgEntity
{
    public string UserId { get; init; }

    public bool HasUserId => string.IsNullOrWhiteSpace(UserId) is false;
}
