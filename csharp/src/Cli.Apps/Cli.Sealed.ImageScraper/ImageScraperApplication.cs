using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.ImageScraper.Dashboard;
using Cli.Sealed.ImageScraper.Downloading;
using Cli.Sealed.ImageScraper.ImageProviders;
using Cli.Sealed.ImageScraper.Logging;
using Cli.Sealed.ImageScraper.MtgJson;
using Cli.Sealed.ImageScraper.MtgoRedemption;
using Cli.Sealed.ImageScraper.Orchestration;
using Example.Core;
using Microsoft.Extensions.Logging;

namespace Cli.Sealed.ImageScraper;

internal sealed class ImageScraperApplication : ExampleApplication
{
    private readonly IReadOnlyList<string> _setCodes;

    public ImageScraperApplication(IReadOnlyList<string> setCodes) => _setCodes = setCodes;

    protected override async Task Execute()
    {
        if (_setCodes is not null && _setCodes.Count == 0)
        {
            Log("Error: No set codes provided. Use --sets MKM,DSK,WOE or --all");
            return;
        }

        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            _ = builder.AddConsole();
            _ = builder.SetMinimumLevel(LogLevel.Warning);
        });

        ILogger logger = loggerFactory.CreateLogger<ImageScraperApplication>();

        using RazorConsoleImageScraperDashboard dashboard = new();

        Task scraperTask = Task.Run(async () =>
        {
            try
            {
                await RunScraperAsync(dashboard).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Swallow exception to pass to UI
            catch (Exception ex)
#pragma warning restore CA1031
            {
                logger.LogError(ex, "Fatal error during image scraping");
                dashboard.MarkComplete($"Failed: {ex.Message}");
            }
        });

        await dashboard.RunUiAsync().ConfigureAwait(false);
        await scraperTask.ConfigureAwait(false);
    }

    private async Task RunScraperAsync(IImageScraperDashboard dashboard)
    {
        using HttpClient httpClient = new()
        {
            Timeout = TimeSpan.FromMinutes(5)
        };
        httpClient.DefaultRequestHeaders.Add("User-Agent", "mtgdiscovery");
        httpClient.DefaultRequestHeaders.Add("Accept", "image/*,*/*;q=0.8");

        IMtgJsonCache cache = new MtgJsonCache();
        IMtgJsonFetcher fetcher = new MtgJsonFetcher(
            httpClient,
            cache,
            status => dashboard.SetStatus(status));

        IReadOnlyList<IImageProvider> imageProviders =
        [
            new TcgPlayerImageProvider(),
            new CardMarketImageProvider(),
            new CardTraderImageProvider(httpClient)
        ];

        IImageDownloader downloader = new ImageDownloader(httpClient);
        IMtgoRedemptionImageGenerator mtgoRedemptionGenerator = new MtgoRedemptionImageGenerator(httpClient);

        using ISkippedProductLogger skippedLogger = new SkippedProductLogger();

        IImageScraperOrchestrator orchestrator = new ImageScraperOrchestrator(
            _setCodes,
            fetcher,
            imageProviders,
            downloader,
            skippedLogger,
            dashboard,
            mtgoRedemptionGenerator);

        CancellationToken cancellationToken = dashboard.GetCancellationToken();
        await orchestrator.ExecuteAsync(cancellationToken).ConfigureAwait(false);
    }
}
