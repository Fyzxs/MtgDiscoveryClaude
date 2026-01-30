using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.MtgDiscovery.PriceUpdate.Dashboard;

internal interface IPriceUpdateDashboard : IDisposable
{
    void SetStatus(string status);
    void SetContainer(string containerName, int containerIndex, int total);
    void UpdateProgress(int current, string cardName);
    void IncrementUpdated();
    void IncrementSkipped();
    void IncrementError();
    void AddRu(double ru);
    void AddLog(string message);
    void StartTimer();
    void MarkComplete(string message);
    CancellationToken GetCancellationToken();
    Task RunUiAsync();
}
