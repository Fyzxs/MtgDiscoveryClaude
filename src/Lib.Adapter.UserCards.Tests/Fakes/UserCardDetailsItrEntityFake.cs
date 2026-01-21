using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class UserCardDetailsItrEntityFake : IUserCardDetailsItrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
