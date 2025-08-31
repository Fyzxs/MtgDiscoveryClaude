using System.IO;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.BulkProcessing.Storage;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Loaders;

internal interface IBulkRulingLoader
{
    Task LoadRulingsAsync(Stream rulingsStream, IBulkRulingStorage rulingStorage);
}