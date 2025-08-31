using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Storage;

internal sealed class BulkRulingsData
{
    public string OracleId { get; }
    public List<BulkRulingEntry> Rulings { get; }

    public BulkRulingsData(string oracleId)
    {
        OracleId = oracleId;
        Rulings = new List<BulkRulingEntry>();
    }

    public void AddRuling(BulkRulingEntry ruling)
    {
        Rulings.Add(ruling);
    }
}