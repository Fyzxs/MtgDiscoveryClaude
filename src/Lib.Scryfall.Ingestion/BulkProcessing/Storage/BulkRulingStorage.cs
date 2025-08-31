using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Storage;

internal sealed class BulkRulingStorage : IBulkRulingStorage
{
    private readonly Dictionary<string, BulkRulingsData> _rulingsByOracleId = new();

    public void AddRuling(string oracleId, BulkRulingEntry ruling)
    {
        if (string.IsNullOrWhiteSpace(oracleId))
        {
            return;
        }

        if (_rulingsByOracleId.TryGetValue(oracleId, out BulkRulingsData existingRulings) is false)
        {
            existingRulings = new BulkRulingsData(oracleId);
            _rulingsByOracleId[oracleId] = existingRulings;
        }

        existingRulings.AddRuling(ruling);
    }

    public IEnumerable<BulkRulingsData> GetAll()
    {
        return _rulingsByOracleId.Values;
    }
}