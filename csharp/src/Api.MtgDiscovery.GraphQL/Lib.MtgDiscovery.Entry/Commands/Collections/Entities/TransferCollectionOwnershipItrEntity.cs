using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Entities;

internal sealed class TransferCollectionOwnershipItrEntity : ITransferCollectionOwnershipItrEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string CurrentOwnerId { get; init; } = string.Empty;
    public string TargetUserId { get; init; } = string.Empty;
}
