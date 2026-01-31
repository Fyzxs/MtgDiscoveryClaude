using System.Threading;
using System.Threading.Tasks;
using Cli.MtgDiscovery.UserDataReconciler.Dashboard.RazorUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RazorConsole.Core;

namespace Cli.MtgDiscovery.UserDataReconciler.Dashboard;

internal sealed class RazorConsoleReconcilerDashboard : IReconcilerDashboard
{
    private readonly ReconcilerDashboardState _state;

    public RazorConsoleReconcilerDashboard() => _state = new ReconcilerDashboardState();

    public async Task RunUiAsync()
    {
        IHostBuilder hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton(_state);
            })
            .UseRazorConsole<ReconcilerDashboard>();

        IHost host = hostBuilder.Build();
        await host.RunAsync().ConfigureAwait(false);
    }

    public void SetStatus(string status) => _state.SetStatus(status);

    public void SetMode(bool isDryRun) => _state.SetMode(isDryRun);

    public void SetTotalUsers(int count) => _state.SetTotalUsers(count);

    public void StartUser(string userId, int userIndex) => _state.StartUser(userId, userIndex);

    public void UpdateUserProgress(string setCode, int setsProcessed, int totalSets) => _state.UpdateUserProgress(setCode, setsProcessed, totalSets);

    public void IncrementSetsProcessed() => _state.IncrementSetsProcessed();

    public void IncrementSetsWithDiscrepancy() => _state.IncrementSetsWithDiscrepancy();

    public void IncrementSetsUpdated() => _state.IncrementSetsUpdated();

    public void AddCardsAdded(int count) => _state.AddCardsAdded(count);

    public void AddCardsRemoved(int count) => _state.AddCardsRemoved(count);

    public void IncrementError() => _state.IncrementError();

    public void AddLog(string message) => _state.AddLog(message);

    public void StartTimer() => _state.StartTimer();

    public void MarkComplete(string message) => _state.MarkComplete(message);

    public CancellationToken GetCancellationToken() => _state.GetCancellationToken();

    public void Dispose() => _state.Dispose();
}
