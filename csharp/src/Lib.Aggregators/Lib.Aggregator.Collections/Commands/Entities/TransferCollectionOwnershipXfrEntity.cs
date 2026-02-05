using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Aggregator.Collections.Commands.Entities;

internal sealed class TransferCollectionOwnershipXfrEntity : ITransferCollectionOwnershipXfrEntity
{
    public string CollectionId { get; init; }
    public string CurrentOwnerId { get; init; }
    public string TargetUserId { get; init; }
    public string CacheKey => $"transfer_ownership:{CollectionId}:from:{CurrentOwnerId}:to:{TargetUserId}";
}
