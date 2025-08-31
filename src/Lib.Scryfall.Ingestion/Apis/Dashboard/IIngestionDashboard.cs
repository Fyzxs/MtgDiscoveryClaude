using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Apis.Dashboard;

public interface IIngestionDashboard : ILogger
{
    void UpdateSetProgress(int current, int total, string name);
    void UpdateCardProgress(int current, int total, string name);
    void UpdateArtistCount(int count);
    void UpdateRulingCount(int count);
    void AddCompletedSet(string name);
    void UpdateMemoryUsage();
    void SetStartTime();
    void Refresh();
    void Complete(string message);
}