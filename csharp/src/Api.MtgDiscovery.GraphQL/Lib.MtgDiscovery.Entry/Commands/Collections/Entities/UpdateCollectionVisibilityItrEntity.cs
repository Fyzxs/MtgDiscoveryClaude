using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Entities;

internal sealed class UpdateCollectionVisibilityItrEntity : IUpdateCollectionVisibilityItrEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string OwnerId { get; init; } = string.Empty;
    public string Visibility { get; init; } = string.Empty;
}
