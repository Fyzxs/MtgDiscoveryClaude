using System.Linq;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.BulkProcessing.Models;
using Lib.Scryfall.Ingestion.Http;
using Lib.Universal.Http;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Fetchers;

internal sealed class BulkDataMetadataFetcher : IBulkDataMetadataFetcher
{
    private const string BulkDataUri = "https://api.scryfall.com/bulk-data";
    private readonly IHttpClient _httpClient;

    public BulkDataMetadataFetcher() : this(new RateLimitedHttpClient())
    {
    }

    private BulkDataMetadataFetcher(IHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IScryfallBulkMetadata> FetchAllCardsMetadataAsync()
    {
        ScryfallBulkMetadataResponse response = await _httpClient.ResponseAs<ScryfallBulkMetadataResponse>(
            new System.Uri(BulkDataUri)).ConfigureAwait(false);

        return response.Data.FirstOrDefault(item => item.Type == "all_cards");
    }

    public async Task<IScryfallBulkMetadata> FetchRulingsMetadataAsync()
    {
        ScryfallBulkMetadataResponse response = await _httpClient.ResponseAs<ScryfallBulkMetadataResponse>(
            new System.Uri(BulkDataUri)).ConfigureAwait(false);

        return response.Data.FirstOrDefault(item => item.Type == "rulings");
    }
}