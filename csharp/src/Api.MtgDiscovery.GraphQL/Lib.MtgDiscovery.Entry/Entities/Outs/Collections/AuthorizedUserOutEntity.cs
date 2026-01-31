namespace Lib.MtgDiscovery.Entry.Entities.Outs.Collections;

public sealed class AuthorizedUserOutEntity
{
    public string UserId { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string GrantedAt { get; init; } = string.Empty;
    public string GrantedBy { get; init; } = string.Empty;
}
