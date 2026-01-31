using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.ImageScraper.Models;
using Cli.Sealed.ImageScraper.MtgJson.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        if (_cache.Exists() is false)
        {
            await DownloadAndCacheAsync(cancellationToken).ConfigureAwait(false);
        }

        _onStatusUpdate("Streaming sealed products from AllPrintings.json...");

        HashSet<string> requestedSets = setCodes is null
            ? null
            : new HashSet<string>(setCodes, StringComparer.OrdinalIgnoreCase);

        List<SealedProduct> products = [];
        int setCount = 0;

        using (FileStream fileStream = new(_cache.GetCachePath(), FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 65536, useAsync: true))
        using (StreamReader streamReader = new(fileStream))
        using (JsonTextReader jsonReader = new(streamReader))
        {
            while (await jsonReader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                if (jsonReader.TokenType == JsonToken.PropertyName && string.Equals(jsonReader.Value?.ToString(), "data", StringComparison.OrdinalIgnoreCase))
                {
                    await jsonReader.ReadAsync(cancellationToken).ConfigureAwait(false);

                    while (await jsonReader.ReadAsync(cancellationToken).ConfigureAwait(false))
                    {
                        if (jsonReader.TokenType == JsonToken.EndObject)
                        {
                            break;
                        }

                        if (jsonReader.TokenType == JsonToken.PropertyName)
                        {
                            string setCode = jsonReader.Value?.ToString();
                            await jsonReader.ReadAsync(cancellationToken).ConfigureAwait(false);

                            if (requestedSets is not null && requestedSets.Contains(setCode) is false)
                            {
                                await jsonReader.SkipAsync(cancellationToken).ConfigureAwait(false);
                                continue;
                            }

                            JObject setObject = await JObject.LoadAsync(jsonReader, cancellationToken).ConfigureAwait(false);

                            if (setObject["sealedProduct"] is not JArray sealedProductArray || sealedProductArray.Count == 0)
                            {
                                continue;
                            }

                            setCount++;
                            string setName = setObject["name"]?.ToString();

                            foreach (JToken item in sealedProductArray)
                            {
                                MtgJsonSealedProductDto dto = item.ToObject<MtgJsonSealedProductDto>();

                                if (dto is not null)
                                {
                                    products.Add(new SealedProduct
                                    {
                                        Uuid = dto.Uuid,
                                        Name = dto.Name,
                                        SetCode = setCode,
                                        SetName = setName,
                                        Category = dto.Category,
                                        Subtype = dto.Subtype,
                                        TcgplayerProductId = dto.Identifiers?.TcgplayerProductId,
                                        McmId = dto.Identifiers?.McmId,
                                        CardTraderId = dto.Identifiers?.CardTraderId
                                    });
                                }
                            }
                        }
                    }

                    break;
                }
            }
        }

        _onStatusUpdate($"Found {products.Count} sealed products across {setCount} sets");

        return products;
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
