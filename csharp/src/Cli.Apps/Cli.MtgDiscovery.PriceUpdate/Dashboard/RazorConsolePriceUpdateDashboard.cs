using System.Threading;
using System.Threading.Tasks;
using Cli.MtgDiscovery.PriceUpdate.Dashboard.RazorUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RazorConsole.Core;

namespace Cli.MtgDiscovery.PriceUpdate.Dashboard;

internal sealed class RazorConsolePriceUpdateDashboard : IPriceUpdateDashboard
{
    private readonly PriceUpdateDashboardState _state;

    public RazorConsolePriceUpdateDashboard() => _state = new PriceUpdateDashboardState();

    public async Task RunUiAsync()
    {
        IHostBuilder hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton(_state);
            })
            .UseRazorConsole<PriceUpdateDashboard>();

        IHost host = hostBuilder.Build();
        await host.RunAsync().ConfigureAwait(false);
    }

    public void SetStatus(string status) => _state.SetStatus(status);

    public void SetContainer(string containerName, int containerIndex, int total) => _state.SetContainer(containerName, containerIndex, total);

    public void UpdateProgress(int current, string cardName) => _state.UpdateProgress(current, cardName);

    public void IncrementUpdated() => _state.IncrementUpdated();

    public void IncrementSkipped() => _state.IncrementSkipped();

    public void IncrementError() => _state.IncrementError();

    public void AddRu(double ru) => _state.AddRu(ru);

    public void AddLog(string message) => _state.AddLog(message);

    public void StartTimer() => _state.StartTimer();

    public void MarkComplete(string message) => _state.MarkComplete(message);

    public CancellationToken GetCancellationToken() => _state.GetCancellationToken();

    public void Dispose() => _state.Dispose();
}
