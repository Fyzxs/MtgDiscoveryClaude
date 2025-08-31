using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.BulkProcessing.Storage;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Writers;

internal interface IBulkRulingWriter
{
    Task WriteRulingsAsync(IEnumerable<BulkRulingsData> rulings);
}