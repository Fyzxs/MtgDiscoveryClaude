using System.Threading;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Apis.Dashboard;

public interface IIngestionDashboard : ILogger
{
    void UpdateProgress(string type, int current, int total, string action, string item);
    void UpdateSetProgress(int current, int total, string name);
    void UpdateCardProgress(int current, int total, string name);
    void UpdateRulingProgress(int current, int total, string name);
    void UpdateTrigramProgress(int current, int total, string name);
    void AddCompletedSet(string name);
    void UpdateMemoryUsage();
    void UpdateCompletedCount(string type, int count);
    void SetStartTime();
    void Refresh();
    void Complete(string message);
    CancellationToken GetCancellationToken();
}
