using System.Collections.Generic;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Aggregator.UserCards.Queries.UserCardsForSigning.Entities;

internal sealed class UserCardsForSigningXfrEntity : IUserCardsForSigningXfrEntity
{
    public string UserId { get; init; }
    public IEnumerable<string> ArtistIds { get; init; }
    public string CacheKey => $"user_cards:signing:{UserId}";
}
