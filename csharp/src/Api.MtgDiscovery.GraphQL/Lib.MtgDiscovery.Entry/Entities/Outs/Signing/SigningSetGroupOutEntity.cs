using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.Signing;

public sealed class SigningSetGroupOutEntity
{
    public string SetId { get; init; }
    public string SetCode { get; init; }
    public string SetName { get; init; }
    public int ArtistCount { get; init; }
    public int UnsignedCardCount { get; init; }
    public string ReleasedAt { get; init; }
    public ICollection<SigningArtistGroupOutEntity> Artists { get; init; }
}
