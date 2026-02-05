using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.Collections.Apis.Entities;

public interface IAuthorizedUserXfrEntity : IXfrEntity
{
    string UserId { get; }
    string Role { get; }
    string GrantedAt { get; }
    string GrantedBy { get; }
}
