using Lib.Adapter.Collections.Apis.Entities;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class OwnerIdXfrEntityFake : IOwnerIdXfrEntity
{
    public string OwnerId { get; init; } = string.Empty;
    public string CacheKey => $"owner:{OwnerId}";
}
