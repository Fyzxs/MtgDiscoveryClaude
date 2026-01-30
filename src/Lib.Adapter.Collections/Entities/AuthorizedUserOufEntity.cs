using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.Adapter.Collections.Entities;

internal sealed class AuthorizedUserOufEntity : IAuthorizedUserOufEntity
{
    public string UserId { get; init; }
    public string Role { get; init; }
    public string GrantedAt { get; init; }
    public string GrantedBy { get; init; }
}
