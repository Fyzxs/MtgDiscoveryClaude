using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Entities;

internal sealed class RevokeCollectionAccessItrEntity : IRevokeCollectionAccessItrEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string RevokerUserId { get; init; } = string.Empty;
    public string TargetUserId { get; init; } = string.Empty;
}
