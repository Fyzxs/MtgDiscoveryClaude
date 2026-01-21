using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class UserCardsSetXfrEntityFake : IUserCardsSetXfrEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
}