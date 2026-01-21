using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.MtgDiscovery.UserDataReconciler.Dashboard;

internal interface IReconcilerDashboard : IDisposable
{
    void SetStatus(string status);
    void SetMode(bool isDryRun);
    void SetTotalUsers(int count);
    void StartUser(string userId, int userIndex);
    void UpdateUserProgress(string setCode, int setsProcessed, int totalSets);
    void IncrementSetsProcessed();
    void IncrementSetsWithDiscrepancy();
    void IncrementSetsUpdated();
    void AddCardsAdded(int count);
    void AddCardsRemoved(int count);
    void IncrementError();
    void AddLog(string message);
    void StartTimer();
    void MarkComplete(string message);
    CancellationToken GetCancellationToken();
    Task RunUiAsync();
}
