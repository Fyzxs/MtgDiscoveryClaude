using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;

namespace Lib.Aggregator.UserCards.Queries.UserCardsForSigning.Entities;

internal sealed class SigningArtistGroupOufEntity : ISigningArtistGroupOufEntity
{
    public string ArtistId { get; init; }
    public string ArtistName { get; init; }
    public int UnsignedCount { get; init; }
    public int PartiallySignedCount { get; init; }
    public IEnumerable<ISigningCardOufEntity> Cards { get; init; }
}
