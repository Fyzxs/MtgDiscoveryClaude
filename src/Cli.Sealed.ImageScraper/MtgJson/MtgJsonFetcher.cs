using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.ImageScraper.MtgJson.Dtos;
using Cli.Sealed.ImageScraper.Models;
using Newtonsoft.Json;

namespace Cli.Sealed.ImageScraper.MtgJson;

internal sealed class MtgJsonFetcher : IMtgJsonFetcher
{
    private const string AllPrintingsZipUrl = "https://mtgjson.com/api/v5/AllPrintings.json.zip";

    private readonly HttpClient _httpClient;
    private readonly IMtgJsonCache _cache;
    private readonly Action<string> _onStatusUpdate;

    public MtgJsonFetcher(
        HttpClient httpClient,
        IMtgJsonCache cache,
        Action<string> onStatusUpdate)
    {
        _httpClient = httpClient;
        _cache = cache;
        _onStatusUpdate = onStatusUpdate;
    }

    public async Task<IReadOnlyList<SealedProduct>> GetSealedProductsAsync(
        IReadOnlyList<string> setCodes,
        CancellationToken cancellationToken)
    {
        AllPrintingsDto allPrintings = await LoadAllPrintingsAsync(cancellationToken).ConfigureAwait(false);

        HashSet<string> requestedSets = setCodes is null
            ? null
            : new HashSet<string>(setCodes, StringComparer.OrdinalIgnoreCase);

        List<SealedProduct> products = [];

        foreach (KeyValuePair<string, MtgJsonSetDto> kvp in allPrintings.Data)
        {
            if (requestedSets is not null && requestedSets.Contains(kvp.Key) is false)
            {
                continue;
            }

            MtgJsonSetDto set = kvp.Value;
            if (set.SealedProduct is null)
            {
                continue;
            }

            foreach (MtgJsonSealedProductDto dto in set.SealedProduct)
            {
                products.Add(new SealedProduct
                {
                    Uuid = dto.Uuid,
                    Name = dto.Name,
                    SetCode = set.Code,
                    SetName = set.Name,
                    Category = dto.Category,
                    ScgId = dto.Identifiers?.ScgId,
                    TcgplayerProductId = dto.Identifiers?.TcgplayerProductId,
                    McmId = dto.Identifiers?.McmId,
                    CardTraderId = dto.Identifiers?.CardTraderId
                });
            }
        }

        return products;
    }

    private async Task<AllPrintingsDto> LoadAllPrintingsAsync(CancellationToken cancellationToken)
    {
        if (_cache.Exists() is false)
        {
            await DownloadAndCacheAsync(cancellationToken).ConfigureAwait(false);
        }

        _onStatusUpdate("Loading AllPrintings.json from cache...");

        string json = await File.ReadAllTextAsync(_cache.GetCachePath(), cancellationToken).ConfigureAwait(false);
        AllPrintingsDto result = JsonConvert.DeserializeObject<AllPrintingsDto>(json);

        _onStatusUpdate($"Loaded {result?.Data?.Count ?? 0} sets from cache");

        return result;
    }

    private async Task DownloadAndCacheAsync(CancellationToken cancellationToken)
    {
        _onStatusUpdate("Downloading AllPrintings.json.zip from MTGJSON...");

        using HttpResponseMessage response = await _httpClient
            .GetAsync(new Uri(AllPrintingsZipUrl), HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        Stream contentStream = await response.Content
            .ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);
        await using (contentStream.ConfigureAwait(false))
        {
            using MemoryStream memoryStream = new();
            await contentStream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
            memoryStream.Position = 0;

            _onStatusUpdate("Extracting AllPrintings.json...");

            await _cache.SaveAsync(memoryStream, cancellationToken).ConfigureAwait(false);

            _onStatusUpdate("AllPrintings.json cached successfully");
        }
    }
}
