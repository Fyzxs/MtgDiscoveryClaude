using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;

namespace Lib.Aggregator.UserCards.Queries.UserCardsForSigning.Entities;

internal sealed class SigningSetGroupOufEntity : ISigningSetGroupOufEntity
{
    public string SetId { get; init; }
    public string SetCode { get; init; }
    public string SetName { get; init; }
    public int ArtistCount { get; init; }
    public int UnsignedCardCount { get; init; }
    public string ReleasedAt { get; init; }
    public IEnumerable<ISigningArtistGroupOufEntity> Artists { get; init; }
}
