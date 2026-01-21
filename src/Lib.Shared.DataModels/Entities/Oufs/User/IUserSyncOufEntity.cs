using System;
using Lib.Shared.DataModels.Abstractions;

namespace Lib.Shared.DataModels.Entities.Oufs.User;

public interface IUserSyncOufEntity : IOufEntity
{
    string UserId { get; }
    string DisplayName { get; }
    string Email { get; }
    DateTime CreatedAt { get; }
    DateTime LastLoginAt { get; }
    bool IsFirstLogin { get; }
}
