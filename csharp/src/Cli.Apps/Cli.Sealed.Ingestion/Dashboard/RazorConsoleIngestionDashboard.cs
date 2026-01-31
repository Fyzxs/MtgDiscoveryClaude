using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.Ingestion.Dashboard.RazorUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RazorConsole.Core;

namespace Cli.Sealed.Ingestion.Dashboard;

internal sealed class RazorConsoleIngestionDashboard : IIngestionDashboard
{
    private readonly IngestionDashboardState _state;

    public RazorConsoleIngestionDashboard() => _state = new IngestionDashboardState();

    public async Task RunUiAsync()
    {
        IHostBuilder hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton(_state);
            })
            .UseRazorConsole<IngestionDashboard>();

        IHost host = hostBuilder.Build();
        await host.RunAsync().ConfigureAwait(false);
    }

    public void SetStatus(string status) => _state.SetStatus(status);

    public void SetCurrentSet(string setCode, int setIndex, int totalSets) => _state.SetCurrentSet(setCode, setIndex, totalSets);

    public void SetTotalProducts(int total) => _state.SetTotalProducts(total);

    public void UpdateProgress(int current, string productName) => _state.UpdateProgress(current, productName);

    public void IncrementSuccess() => _state.IncrementSuccess();

    public void IncrementSkipped() => _state.IncrementSkipped();

    public void IncrementError() => _state.IncrementError();

    public void AddLog(string message) => _state.AddLog(message);

    public void StartTimer() => _state.StartTimer();

    public void MarkComplete(string message) => _state.MarkComplete(message);

    public CancellationToken GetCancellationToken() => _state.GetCancellationToken();

    public void Dispose() => _state.Dispose();
}
