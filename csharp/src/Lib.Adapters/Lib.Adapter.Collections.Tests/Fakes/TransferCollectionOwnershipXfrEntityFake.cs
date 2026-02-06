using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class TransferCollectionOwnershipXfrEntityFake : ITransferCollectionOwnershipXfrEntity
{
    public string CollectionId { get; init; } = string.Empty;
    public string CurrentOwnerId { get; init; } = string.Empty;
    public string TargetUserId { get; init; } = string.Empty;
    public string CacheKey => $"transfer-ownership:{CollectionId}:from:{CurrentOwnerId}:to:{TargetUserId}";
}
