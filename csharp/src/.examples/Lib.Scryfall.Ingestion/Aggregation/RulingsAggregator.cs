using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Lib.Scryfall.Ingestion.Models;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Aggregation;

internal sealed class RulingsAggregator : IRulingsAggregator
{
    public Dictionary<string, IScryfallRuling> AggregateByOracleId(IEnumerable<dynamic> rawRulings)
    {
        Dictionary<string, IScryfallRuling> aggregatedRulings = [];

        foreach (dynamic ruling in rawRulings)
        {
            string oracleId = ruling.oracle_id;

            if (aggregatedRulings.ContainsKey(oracleId) is false)
            {
                aggregatedRulings[oracleId] = new ScryfallRuling()
                {
                    OracleId = oracleId,
                    Rulings = []
                };
            }

            aggregatedRulings[oracleId].Rulings.Add(ruling);
        }

        return aggregatedRulings;
    }
}
