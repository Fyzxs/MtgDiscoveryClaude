namespace Lib.Scryfall.Ingestion.Apis.Dashboard;

public interface IDashboardFactory
{
    IIngestionDashboard Create(ILogger logger);
}
