using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Storage;

internal interface IBulkRulingStorage
{
    void AddRuling(string oracleId, BulkRulingEntry ruling);
    IEnumerable<BulkRulingsData> GetAll();
}