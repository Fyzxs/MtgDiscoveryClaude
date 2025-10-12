using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Aggregator.UserCards.Queries.Entities;

internal sealed class UserCardXfrEntity : IUserCardXfrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
}
