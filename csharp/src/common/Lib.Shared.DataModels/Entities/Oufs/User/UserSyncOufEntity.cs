using System;

namespace Lib.Shared.DataModels.Entities.Oufs.User;

public sealed class UserSyncOufEntity : IUserSyncOufEntity
{
    public string UserId { get; init; }
    public string DisplayName { get; init; }
    public string Email { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastLoginAt { get; init; }
    public bool IsFirstLogin { get; init; }
}
