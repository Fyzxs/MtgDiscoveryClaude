using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Aggregator.UserCards.Queries.Entities;

internal sealed class UserCardsNameXfrEntity : IUserCardsNameXfrEntity
{
    public string UserId { get; init; }
    public string CardName { get; init; }
}
