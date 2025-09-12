using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.Apis.Aggregation;

public interface ICardNameTrigramAggregate
{
    string Trigram();
    IEnumerable<ICardNameTrigramEntry> Entries();
}

public interface ICardNameTrigramEntry
{
    string Name();
    string Normalized();
    IEnumerable<int> Positions();
}
