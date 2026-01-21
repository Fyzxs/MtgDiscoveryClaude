using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Aggregator.UserCards.Queries.Entities;

internal sealed class UserCardsSetXfrEntity : IUserCardsSetXfrEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
}
