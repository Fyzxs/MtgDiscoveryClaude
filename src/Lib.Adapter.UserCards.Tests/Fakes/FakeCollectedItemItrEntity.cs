using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class FakeCollectedItemItrEntity : ICollectedItemItrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
