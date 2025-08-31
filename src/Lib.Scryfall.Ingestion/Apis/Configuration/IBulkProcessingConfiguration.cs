namespace Lib.Scryfall.Ingestion.Apis.Configuration;

public interface IBulkProcessingConfiguration
{
    bool EnableMemoryThrottling { get; }
    int DashboardRefreshFrequency { get; }
}