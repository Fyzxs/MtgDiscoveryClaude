using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Aggregator.Collections.Queries.Entities;

internal sealed class OwnerIdXfrEntity : IOwnerIdXfrEntity
{
    public string OwnerId { get; init; }
    public string CacheKey => $"owner:{OwnerId}";
}
