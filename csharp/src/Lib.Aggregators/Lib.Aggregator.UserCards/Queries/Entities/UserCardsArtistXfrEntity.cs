using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Aggregator.UserCards.Queries.Entities;

internal sealed class UserCardsArtistXfrEntity : IUserCardsArtistXfrEntity
{
    public string UserId { get; init; }
    public string ArtistId { get; init; }
    public string CacheKey => $"user_cards:artist:{UserId}:{ArtistId}";
}
