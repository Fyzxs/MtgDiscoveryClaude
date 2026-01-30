using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.Aggregator.UserCards.Queries.UserCardsForSigning.Entities;

internal sealed class UserCardsForSigningItrEntity : IUserCardsForSigningItrEntity
{
    public string UserId { get; init; }
    public IEnumerable<string> ArtistIds { get; init; }
}
