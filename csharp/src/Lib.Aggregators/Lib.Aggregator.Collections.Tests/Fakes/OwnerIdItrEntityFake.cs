using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Aggregator.Collections.Tests.Fakes;

public sealed class OwnerIdItrEntityFake : IOwnerIdItrEntity
{
    public string OwnerId { get; init; }
}
