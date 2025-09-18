using System.Collections.Generic;
using Lib.Adapter.Artists.Apis.Entities;

namespace Lib.Aggregator.Artists.Queries.Entities;

internal sealed class ArtistSearchTermXfrEntity : IArtistSearchTermXfrEntity
{
    public ICollection<string> SearchTerms { get; set; }
    public string Normalized { get; set; }
}
