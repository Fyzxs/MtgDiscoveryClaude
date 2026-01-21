using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.ImageScraper.Dashboard;
using Cli.Sealed.ImageScraper.Downloading;
using Cli.Sealed.ImageScraper.ImageProviders;
using Cli.Sealed.ImageScraper.Logging;
using Cli.Sealed.ImageScraper.Models;
using Cli.Sealed.ImageScraper.MtgJson;
using Cli.Sealed.ImageScraper.MtgoRedemption;

namespace Cli.Sealed.ImageScraper.Orchestration;

internal sealed class ImageScraperOrchestrator : IImageScraperOrchestrator
{
    private const string OutputDirectory = "sealed-images";
    private const string MtgoRedemptionSuffix = "MTGO Redemption";
    private const string MtgoRedemptionFoilSuffix = "MTGO Redemption Foil";
    private const string PlaceholderImageFileName = "COMING_SOON.jpg";

    private static readonly HashSet<string> s_excludedCategories = new(StringComparer.OrdinalIgnoreCase)
    {
        "booster_case",
        "bundle_case",
        "deck_box",
        "deck"
    };

    private readonly IReadOnlyList<string> _setCodes;
    private readonly IMtgJsonFetcher _mtgJsonFetcher;
    private readonly IReadOnlyList<IImageProvider> _imageProviders;
    private readonly IImageDownloader _imageDownloader;
    private readonly ISkippedProductLogger _skippedLogger;
    private readonly IImageScraperDashboard _dashboard;
    private readonly IMtgoRedemptionImageGenerator _mtgoRedemptionGenerator;

    public ImageScraperOrchestrator(
        IReadOnlyList<string> setCodes,
        IMtgJsonFetcher mtgJsonFetcher,
        IReadOnlyList<IImageProvider> imageProviders,
        IImageDownloader imageDownloader,
        ISkippedProductLogger skippedLogger,
        IImageScraperDashboard dashboard,
        IMtgoRedemptionImageGenerator mtgoRedemptionGenerator)
    {
        _setCodes = setCodes;
        _mtgJsonFetcher = mtgJsonFetcher;
        _imageProviders = imageProviders;
        _imageDownloader = imageDownloader;
        _skippedLogger = skippedLogger;
        _dashboard = dashboard;
        _mtgoRedemptionGenerator = mtgoRedemptionGenerator;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _dashboard.StartTimer();
        _dashboard.SetStatus("Fetching sealed products from MTGJSON...");

        IReadOnlyList<SealedProduct> allProducts = await _mtgJsonFetcher
            .GetSealedProductsAsync(_setCodes, cancellationToken)
            .ConfigureAwait(false);

        ILookup<string, SealedProduct> productsBySet = allProducts.ToLookup(p => p.SetCode);

        IReadOnlyList<string> setsToProcess = _setCodes ?? productsBySet.Select(g => g.Key).OrderBy(s => s).ToList();

        _dashboard.AddLog($"Found {allProducts.Count} sealed products across {setsToProcess.Count} sets");

        int setIndex = 0;

        foreach (string setCode in setsToProcess)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            setIndex++;
            IReadOnlyList<SealedProduct> setProducts = productsBySet[setCode].ToList();

            _dashboard.SetCurrentSet(setCode, setIndex, setsToProcess.Count);
            _dashboard.SetTotalProducts(setProducts.Count);
            _dashboard.AddLog($"Processing set {setCode}: {setProducts.Count} products");

            await ProcessSetAsync(setCode, setProducts, cancellationToken).ConfigureAwait(false);
        }

        string completionMessage = cancellationToken.IsCancellationRequested
            ? "Cancelled by user"
            : "Completed successfully";

        _dashboard.MarkComplete(completionMessage);
    }

    private async Task ProcessSetAsync(
        string setCode,
        IReadOnlyList<SealedProduct> products,
        CancellationToken cancellationToken)
    {
        int productIndex = 0;

        foreach (SealedProduct product in products)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            productIndex++;
            _dashboard.UpdateProgress(productIndex, product.Name);

            if (string.IsNullOrEmpty(product.Category) is false && s_excludedCategories.Contains(product.Category))
            {
                _skippedLogger.LogSkippedKnown(product, $"Excluded category: {product.Category}");
                _dashboard.IncrementSkipped();
                continue;
            }

#pragma warning disable CA1308 // SetCode in output path should be lowercase to match URL pattern
            string outputPath = Path.Combine(OutputDirectory, setCode.ToLowerInvariant(), $"{product.Uuid}.jpg");
#pragma warning restore CA1308

            if (File.Exists(outputPath))
            {
                _dashboard.IncrementSkipped();
                continue;
            }

            bool downloaded = await TryDownloadProductImageAsync(product, setCode, outputPath, cancellationToken)
                .ConfigureAwait(false);

            if (downloaded is false)
            {
                _skippedLogger.LogSkippedNoImage(product);
                CopyPlaceholderImage(outputPath);
                _dashboard.IncrementNoImage();
            }
        }
    }

    private async Task<bool> TryDownloadProductImageAsync(
        SealedProduct product,
        string setCode,
        string outputPath,
        CancellationToken cancellationToken)
    {
        if (IsMtgoRedemption(product.Name, out bool isFoil))
        {
            bool generated = await _mtgoRedemptionGenerator
                .GenerateAsync(setCode, product.SetName, isFoil, outputPath, cancellationToken)
                .ConfigureAwait(false);

            if (generated)
            {
                _dashboard.IncrementDownloaded();
                _dashboard.AddLog($"Generated: {product.Name} (MTGO Redemption)");
                return true;
            }
        }

        return await TryDownloadFromProvidersAsync(product, outputPath, cancellationToken)
            .ConfigureAwait(false);
    }

    private static bool IsMtgoRedemption(string productName, out bool isFoil)
    {
        isFoil = false;

        if (string.IsNullOrEmpty(productName))
        {
            return false;
        }

        if (productName.EndsWith(MtgoRedemptionFoilSuffix, StringComparison.OrdinalIgnoreCase))
        {
            isFoil = true;
            return true;
        }

        return productName.EndsWith(MtgoRedemptionSuffix, StringComparison.OrdinalIgnoreCase);
    }

    private async Task<bool> TryDownloadFromProvidersAsync(
        SealedProduct product,
        string outputPath,
        CancellationToken cancellationToken)
    {
        foreach (IImageProvider provider in _imageProviders)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            if (provider.CanProvide(product) is false)
            {
                continue;
            }

            bool downloaded = await TryDownloadFromProviderAsync(provider, product, outputPath, cancellationToken)
                .ConfigureAwait(false);

            if (downloaded)
            {
                return true;
            }
        }

        return false;
    }

    private async Task<bool> TryDownloadFromProviderAsync(
        IImageProvider provider,
        SealedProduct product,
        string outputPath,
        CancellationToken cancellationToken)
    {
        foreach (ImageSize size in ImageSize.AllDescending)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            IReadOnlyList<string> urls = await provider
                .GetImageUrlsAsync(product, size, cancellationToken)
                .ConfigureAwait(false);

            foreach (string url in urls)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return false;
                }

                DownloadResult result = await _imageDownloader
                    .DownloadAsync(url, outputPath, cancellationToken)
                    .ConfigureAwait(false);

                if (result.IsSuccess)
                {
                    _dashboard.IncrementDownloaded();
                    _dashboard.AddLog($"Downloaded: {product.Name} ({size}) from {provider.Name}");
                    return true;
                }
            }
        }

        return false;
    }

    private static void CopyPlaceholderImage(string outputPath)
    {
        string directory = Path.GetDirectoryName(outputPath);
        if (string.IsNullOrEmpty(directory) is false && Directory.Exists(directory) is false)
        {
            _ = Directory.CreateDirectory(directory);
        }

        string placeholderPath = Path.Combine(AppContext.BaseDirectory, PlaceholderImageFileName);
        if (File.Exists(placeholderPath))
        {
            File.Copy(placeholderPath, outputPath, overwrite: true);
        }
    }
}
