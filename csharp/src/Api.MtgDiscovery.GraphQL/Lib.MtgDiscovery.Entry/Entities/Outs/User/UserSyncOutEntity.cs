using System;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.User;

public sealed class UserSyncOutEntity
{
    public string UserId { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime LastLoginAt { get; init; }
    public bool IsFirstLogin { get; init; }
}
