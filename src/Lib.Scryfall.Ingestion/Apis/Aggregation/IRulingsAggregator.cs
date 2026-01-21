using System.Collections.Generic;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Aggregation;

public interface IRulingsAggregator
{
    Dictionary<string, IScryfallRuling> AggregateByOracleId(IEnumerable<dynamic> rawRulings);
}
