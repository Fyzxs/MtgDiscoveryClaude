using System;
using System.IO;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.BulkProcessing.Models;
using Lib.Scryfall.Ingestion.Http;
using Lib.Universal.Http;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Fetchers;

internal sealed class BulkDataDownloader : IBulkDataDownloader
{
    private readonly IHttpClient _httpClient;

    public BulkDataDownloader() : this(new RateLimitedHttpClient())
    {
    }

    private BulkDataDownloader(IHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Stream> DownloadAsync(IScryfallBulkMetadata metadata)
    {
        if (metadata == null || string.IsNullOrWhiteSpace(metadata.DownloadUri))
        {
            throw new ArgumentException("Invalid metadata or download URI");
        }

        return await _httpClient.StreamAsync(new Uri(metadata.DownloadUri)).ConfigureAwait(false);
    }
}