using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Domain.Collections.Tests.Fakes;

public sealed class OwnerIdItrEntityFake : IOwnerIdItrEntity
{
    public string OwnerId { get; init; }
}
