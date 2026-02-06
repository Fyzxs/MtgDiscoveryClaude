using Lib.Adapter.UserSetCards.Apis.Entities;

namespace Lib.Aggregator.UserSetCards.Queries.Entities;

internal sealed class UserSetCardXfrEntity : IUserSetCardGetXfrEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
    public string CacheKey => $"user_set_card:{UserId}:{SetId}";
}
