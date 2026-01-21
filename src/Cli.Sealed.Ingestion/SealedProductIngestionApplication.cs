using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.Ingestion.Dashboard;
using Cli.Sealed.Ingestion.Dtos;
using Cli.Sealed.Ingestion.Ingestion;
using Example.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cli.Sealed.Ingestion;

internal sealed class SealedProductIngestionApplication : ExampleApplication
{
    private const string AllPrintingsUrl = "https://mtgjson.com/api/v5/AllPrintings.json.zip";
    private const string CacheDirectory = ".mtgjson-cache";
    private const string CacheFileName = "AllPrintings.json";

    private readonly IReadOnlyList<string> _setCodes;

    public SealedProductIngestionApplication(string[] args) =>
        _setCodes = ParseSetCodes(args ?? Array.Empty<string>());

    protected override async Task Execute()
    {
        if (_setCodes is not null && _setCodes.Count == 0)
        {
            Log("Usage: dotnet run -- --sets MKM,DSK,WOE");
            Log("       dotnet run -- --all");
            return;
        }

        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            _ = builder.AddConsole();
            _ = builder.SetMinimumLevel(LogLevel.Warning);
        });

        ILogger logger = loggerFactory.CreateLogger<SealedProductIngestionApplication>();

        using RazorConsoleIngestionDashboard dashboard = new();

        Task ingestionTask = Task.Run(async () =>
        {
            try
            {
                await RunIngestionAsync(dashboard, logger).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Swallow exception to pass to UI
            catch (Exception ex)
#pragma warning restore CA1031
            {
                logger.LogError(ex, "Fatal error during ingestion");
                dashboard.MarkComplete($"Failed: {ex.Message}");
            }
        });

        await dashboard.RunUiAsync().ConfigureAwait(false);
        await ingestionTask.ConfigureAwait(false);
    }

    private async Task RunIngestionAsync(IIngestionDashboard dashboard, ILogger logger)
    {
        dashboard.StartTimer();
        CancellationToken cancellationToken = dashboard.GetCancellationToken();

        using HttpClient httpClient = new() { Timeout = TimeSpan.FromMinutes(10) };
        httpClient.DefaultRequestHeaders.Add("User-Agent", "mtgdiscovery-ingestion");

        dashboard.SetStatus("Loading AllPrintings.json...");
        AllPrintingsDto allPrintings = await LoadAllPrintingsAsync(httpClient, dashboard).ConfigureAwait(false);

        if (allPrintings?.Data is null)
        {
            dashboard.MarkComplete("Failed: Could not load AllPrintings data");
            return;
        }

        dashboard.AddLog($"Loaded {allPrintings.Data.Count:N0} sets from MTGJSON");

        ISealedProductIngestionService ingestionService = new SealedProductIngestionService(logger);

        List<KeyValuePair<string, MtgJsonSetDto>> setsToProcess = GetSetsToProcess(allPrintings);

        if (setsToProcess.Count == 0)
        {
            dashboard.MarkComplete("No sets with sealed products found");
            return;
        }

        dashboard.SetStatus("Ingesting sealed products...");
        int setIndex = 0;

        foreach (KeyValuePair<string, MtgJsonSetDto> kvp in setsToProcess)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                dashboard.MarkComplete("Cancelled by user");
                return;
            }

            string setCode = kvp.Key;
            MtgJsonSetDto set = kvp.Value;

            setIndex++;
            dashboard.SetCurrentSet(setCode, setIndex, setsToProcess.Count);

            List<MtgJsonSealedProductDto> products = GetProductsToIngest(set);
            dashboard.SetTotalProducts(products.Count);

            int productIndex = 0;
            foreach (MtgJsonSealedProductDto product in products)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    dashboard.MarkComplete("Cancelled by user");
                    return;
                }

                productIndex++;
                dashboard.UpdateProgress(productIndex, product.Name);

                bool success = await ingestionService
                    .IngestProductAsync(setCode, set.Name, product, cancellationToken)
                    .ConfigureAwait(false);

                if (success)
                {
                    dashboard.IncrementSuccess();
                }
                else
                {
                    dashboard.IncrementError();
                    dashboard.AddLog($"Failed: {product.Name}");
                }
            }

            dashboard.AddLog($"Completed {setCode}: {products.Count} products");
        }

        dashboard.MarkComplete("Ingestion complete!");
    }

    private List<KeyValuePair<string, MtgJsonSetDto>> GetSetsToProcess(AllPrintingsDto allPrintings)
    {
        List<KeyValuePair<string, MtgJsonSetDto>> result = [];

        foreach (KeyValuePair<string, MtgJsonSetDto> kvp in allPrintings.Data)
        {
            string setCode = kvp.Key;
            MtgJsonSetDto set = kvp.Value;

            if (_setCodes is not null && Contains(_setCodes, setCode) is false)
            {
                continue;
            }

            if (set.SealedProduct is null || set.SealedProduct.Count == 0)
            {
                continue;
            }

            result.Add(kvp);
        }

        return result;
    }

    private static List<MtgJsonSealedProductDto> GetProductsToIngest(MtgJsonSetDto set)
    {
        List<MtgJsonSealedProductDto> result = [];

        foreach (MtgJsonSealedProductDto product in set.SealedProduct)
        {
            if (string.Equals(product.Category, "booster_case", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            result.Add(product);
        }

        return result;
    }

    private static bool Contains(IReadOnlyList<string> list, string value)
    {
        foreach (string item in list)
        {
            if (string.Equals(item, value, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private async Task<AllPrintingsDto> LoadAllPrintingsAsync(HttpClient httpClient, IIngestionDashboard dashboard)
    {
        string cachePath = Path.Combine(CacheDirectory, CacheFileName);

        if (File.Exists(cachePath) is false)
        {
            dashboard.SetStatus("Downloading AllPrintings.json.zip...");
            await DownloadAndCacheAsync(httpClient, cachePath, dashboard).ConfigureAwait(false);
        }
        else
        {
            dashboard.AddLog("Using cached AllPrintings.json");
        }

        dashboard.SetStatus("Parsing AllPrintings.json...");
        string json = await File.ReadAllTextAsync(cachePath).ConfigureAwait(false);
        AllPrintingsDto result = JsonConvert.DeserializeObject<AllPrintingsDto>(json);

        return result;
    }

    private static async Task DownloadAndCacheAsync(HttpClient httpClient, string cachePath, IIngestionDashboard dashboard)
    {
        using HttpResponseMessage response = await httpClient
            .GetAsync(new Uri(AllPrintingsUrl), HttpCompletionOption.ResponseHeadersRead)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        using Stream contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        using MemoryStream memoryStream = new();
        await contentStream.CopyToAsync(memoryStream).ConfigureAwait(false);
        memoryStream.Position = 0;

        dashboard.SetStatus("Extracting AllPrintings.json...");

        string directory = Path.GetDirectoryName(cachePath);
        if (string.IsNullOrEmpty(directory) is false && Directory.Exists(directory) is false)
        {
            _ = Directory.CreateDirectory(directory);
        }

        using ZipArchive archive = new(memoryStream, ZipArchiveMode.Read);
        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            if (entry.Name.Equals(CacheFileName, StringComparison.OrdinalIgnoreCase))
            {
                await entry.ExtractToFileAsync(cachePath, overwrite: true).ConfigureAwait(false);
                break;
            }
        }

        dashboard.AddLog("Downloaded and cached AllPrintings.json");
    }

    private static IReadOnlyList<string> ParseSetCodes(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Equals("--all", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (args[i].Equals("--sets", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                string[] codes = args[i + 1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                List<string> result = new(codes.Length);
                foreach (string code in codes)
                {
                    result.Add(code.ToUpperInvariant());
                }

                return result;
            }
        }

        return Array.Empty<string>();
    }
}
