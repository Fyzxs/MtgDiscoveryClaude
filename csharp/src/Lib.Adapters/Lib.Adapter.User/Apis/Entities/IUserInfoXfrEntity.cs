using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.User.Apis.Entities;

/// <summary>
/// Transfer entity for user information.
/// Used by adapter operations for user registration/sync.
/// </summary>
public interface IUserInfoXfrEntity : IXfrEntity
{
    string UserId { get; }
    string UserSourceId { get; }
    string UserNickname { get; }
    string Email { get; }
}
