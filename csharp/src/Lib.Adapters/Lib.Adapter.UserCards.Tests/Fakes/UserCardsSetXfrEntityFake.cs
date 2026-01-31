using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Tests.Fakes;

public sealed class UserCardsSetXfrEntityFake : IUserCardsSetXfrEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
}
