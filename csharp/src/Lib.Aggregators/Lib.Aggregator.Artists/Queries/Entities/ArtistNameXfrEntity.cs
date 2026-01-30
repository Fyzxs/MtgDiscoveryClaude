using System.Collections.Generic;
using Lib.Adapter.Artists.Apis.Entities;

namespace Lib.Aggregator.Artists.Queries.Entities;

internal sealed class ArtistNameXfrEntity : IArtistNameXfrEntity
{
    public string ArtistName { get; init; }
    public string Normalized { get; init; }
    public ICollection<string> Trigrams { get; init; }
}
