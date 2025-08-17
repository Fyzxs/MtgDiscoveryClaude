using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.Apis.Aggregation;

public interface IArtistAggregate
{
    string ArtistId();
    IEnumerable<string> ArtistNames();
    IEnumerable<string> CardIds();
    IEnumerable<string> SetIds();
}