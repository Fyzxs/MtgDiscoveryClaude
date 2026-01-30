using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Entities;

internal sealed class GrantCollectionAccessItrEntity : IGrantCollectionAccessItrEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string GrantorUserId { get; init; } = string.Empty;
    public string TargetUserId { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}
