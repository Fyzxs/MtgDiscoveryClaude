using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.ImageScraper.Dashboard.RazorUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RazorConsole.Core;

namespace Cli.Sealed.ImageScraper.Dashboard;

internal sealed class RazorConsoleImageScraperDashboard : IImageScraperDashboard
{
    private readonly ImageScraperDashboardState _state;

    public RazorConsoleImageScraperDashboard() => _state = new ImageScraperDashboardState();

    public async Task RunUiAsync()
    {
        IHostBuilder hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton(_state);
            })
            .UseRazorConsole<ImageScraperDashboard>();

        IHost host = hostBuilder.Build();
        await host.RunAsync().ConfigureAwait(false);
    }

    public void SetStatus(string status) => _state.SetStatus(status);

    public void SetCurrentSet(string setCode, int setIndex, int totalSets) =>
        _state.SetCurrentSet(setCode, setIndex, totalSets);

    public void SetTotalProducts(int total) => _state.SetTotalProducts(total);

    public void UpdateProgress(int current, string productName) =>
        _state.UpdateProgress(current, productName);

    public void IncrementDownloaded() => _state.IncrementDownloaded();

    public void IncrementSkipped() => _state.IncrementSkipped();

    public void IncrementError() => _state.IncrementError();

    public void IncrementNoImage() => _state.IncrementNoImage();

    public void AddLog(string message) => _state.AddLog(message);

    public void StartTimer() => _state.StartTimer();

    public void MarkComplete(string message) => _state.MarkComplete(message);

    public CancellationToken GetCancellationToken() => _state.GetCancellationToken();

    public void Dispose() => _state.Dispose();
}
