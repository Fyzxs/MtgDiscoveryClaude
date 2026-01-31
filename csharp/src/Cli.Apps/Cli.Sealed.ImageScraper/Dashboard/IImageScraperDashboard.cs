using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Sealed.ImageScraper.Dashboard;

internal interface IImageScraperDashboard : IDisposable
{
    void SetStatus(string status);
    void SetCurrentSet(string setCode, int setIndex, int totalSets);
    void SetTotalProducts(int total);
    void UpdateProgress(int current, string productName);
    void IncrementDownloaded();
    void IncrementSkipped();
    void IncrementError();
    void IncrementNoImage();
    void AddLog(string message);
    void StartTimer();
    void MarkComplete(string message);
    CancellationToken GetCancellationToken();
    Task RunUiAsync();
}
