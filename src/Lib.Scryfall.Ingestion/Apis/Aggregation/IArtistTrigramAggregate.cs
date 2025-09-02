using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.Apis.Aggregation;

public interface IArtistTrigramAggregate
{
    string Trigram();
    IEnumerable<IArtistTrigramEntry> Entries();
}

public interface IArtistTrigramEntry
{
    string ArtistId();
    string Name();
    string Normalized();
    IEnumerable<int> Positions();
}