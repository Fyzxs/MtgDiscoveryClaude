using System.IO;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.BulkProcessing.Models;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Fetchers;

internal interface IBulkDataDownloader
{
    Task<Stream> DownloadAsync(IScryfallBulkMetadata metadata);
}