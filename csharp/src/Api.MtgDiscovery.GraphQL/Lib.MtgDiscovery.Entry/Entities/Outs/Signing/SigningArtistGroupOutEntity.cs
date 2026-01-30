using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.Signing;

public sealed class SigningArtistGroupOutEntity
{
    public string ArtistId { get; init; }
    public string ArtistName { get; init; }
    public int UnsignedCount { get; init; }
    public int PartiallySignedCount { get; init; }
    public ICollection<SigningCardOutEntity> Cards { get; init; }
}
