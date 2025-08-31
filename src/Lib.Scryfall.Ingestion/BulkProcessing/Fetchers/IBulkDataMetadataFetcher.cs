using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.BulkProcessing.Models;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Fetchers;

internal interface IBulkDataMetadataFetcher
{
    Task<IScryfallBulkMetadata> FetchAllCardsMetadataAsync();
    Task<IScryfallBulkMetadata> FetchRulingsMetadataAsync();
}