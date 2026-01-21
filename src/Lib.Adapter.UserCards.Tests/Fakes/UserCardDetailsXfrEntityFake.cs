using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class UserCardDetailsXfrEntityFake : IUserCardDetailsXfrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
    public string SetGroupId { get; init; }
}
