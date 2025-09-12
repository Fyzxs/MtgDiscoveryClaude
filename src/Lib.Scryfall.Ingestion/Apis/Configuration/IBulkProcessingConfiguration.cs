using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.Apis.Configuration;

public interface IBulkProcessingConfiguration
{
    bool EnableMemoryThrottling { get; }
    int DashboardRefreshFrequency { get; }
    bool ProcessRulings { get; }
    bool SetsOnly { get; }
    IReadOnlyCollection<string> SetCodesToProcess { get; }
}
