using Lib.Adapter.UserSetCards.Apis.Entities;

namespace Lib.Aggregator.UserSetCards.Queries.Entities;

internal sealed class AllUserSetCardsXfrEntity : IAllUserSetCardsXfrEntity
{
    public string UserId { get; init; }
    public string CacheKey => $"user_set_cards:all:{UserId}";
}
