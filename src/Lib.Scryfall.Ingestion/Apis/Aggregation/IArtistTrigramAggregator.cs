using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.Apis.Aggregation;

public interface IArtistTrigramAggregator
{
    void TrackArtist(string artistId, IEnumerable<string> artistNames);
    IEnumerable<IArtistTrigramAggregate> GetTrigrams();
    void Clear();
}