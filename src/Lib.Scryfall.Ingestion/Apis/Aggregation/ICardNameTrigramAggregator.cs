using System.Collections.Generic;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Aggregation;

public interface ICardNameTrigramAggregator
{
    void Track(IScryfallCard card);
    IEnumerable<ICardNameTrigramAggregate> GetTrigrams();
    void Clear();
}