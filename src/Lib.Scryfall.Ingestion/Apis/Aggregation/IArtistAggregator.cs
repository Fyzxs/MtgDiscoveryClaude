using System.Collections.Generic;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Aggregation;

public interface IArtistAggregator
{
    void Track(IScryfallCard card);
    IEnumerable<IArtistAggregate> GetArtists();
    IEnumerable<IArtistAggregate> GetDirtyArtists();
    void MarkAllClean();
    void Clear();
}
