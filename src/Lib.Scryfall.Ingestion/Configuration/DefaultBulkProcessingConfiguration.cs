using Lib.Scryfall.Ingestion.Apis.Configuration;

namespace Lib.Scryfall.Ingestion.Configuration;

internal sealed class DefaultBulkProcessingConfiguration : IBulkProcessingConfiguration
{
    public bool EnableMemoryThrottling => true;
    public int DashboardRefreshFrequency => 100;
    public bool ProcessRulings => false;
}