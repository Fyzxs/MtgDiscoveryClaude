using System.Collections.Generic;
using System.Linq;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Models;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Aggregation;

internal sealed class RulingsAggregator : IRulingsAggregator
{
    public Dictionary<string, IScryfallRuling> AggregateByOracleId(IEnumerable<dynamic> rawRulings)
    {
        Dictionary<string, List<dynamic>> rulingsByOracle = new();

        foreach (dynamic ruling in rawRulings)
        {
            string oracleId = ruling.oracle_id;

            if (rulingsByOracle.ContainsKey(oracleId) is false)
            {
                rulingsByOracle[oracleId] = new List<dynamic>();
            }
            rulingsByOracle[oracleId].Add(ruling);
        }

        Dictionary<string, IScryfallRuling> aggregatedRulings = new();
        foreach (KeyValuePair<string, List<dynamic>> kvp in rulingsByOracle)
        {
            AggregatedRulingData aggregatedData = new()
            {
                OracleId = kvp.Key,
                Rulings = kvp.Value.ToArray()
            };
            aggregatedRulings[kvp.Key] = new ScryfallRuling(new ExtScryfallRulingDto(aggregatedData));
        }

        return aggregatedRulings;
    }
}
