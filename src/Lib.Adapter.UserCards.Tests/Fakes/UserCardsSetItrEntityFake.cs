using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class UserCardsSetItrEntityFake : IUserCardsSetItrEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
}