using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Entities;

internal sealed class AuthorizedUserItrEntity : IAuthorizedUserItrEntity
{
    public string UserId { get; init; }
    public string Role { get; init; }
    public string GrantedAt { get; init; }
    public string GrantedBy { get; init; }
}
