using System;
using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Apis.Configuration;

namespace Lib.Scryfall.Ingestion.Configuration;

internal sealed class DefaultBulkProcessingConfiguration : IBulkProcessingConfiguration
{
    public bool EnableMemoryThrottling => true;
    public int DashboardRefreshFrequency => 100;
    public bool ProcessRulings => false;
    public bool SetsOnly => false;
    public bool UseRazorConsole => false;
    public IReadOnlyCollection<string> SetCodesToProcess => [];
    public DateTime? SetsReleasedAfter => null;
}
