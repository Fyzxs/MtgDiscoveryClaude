using System.Collections.Generic;
using System.Linq;
using Lib.Scryfall.Ingestion.Apis.Configuration;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Scryfall.Ingestion.Configuration;

internal sealed class ConfigBulkProcessingConfiguration : IBulkProcessingConfiguration
{
    private const string BulkProcessingKey = "BulkProcessing";
    private const string EnableMemoryThrottlingKey = "EnableMemoryThrottling";
    private const string DashboardRefreshFrequencyKey = "DashboardRefreshFrequency";
    private const string ProcessRulingsKey = "ProcessRulings";
    private const string SetsOnlyKey = "SetsOnly";
    private const string SetCodesToProcessKey = "SetCodesToProcess";

    private readonly IConfig _config;

    public ConfigBulkProcessingConfiguration() : this(new MonoStateConfig())
    {
    }

    private ConfigBulkProcessingConfiguration(IConfig config)
    {
        _config = config;
    }

    public bool EnableMemoryThrottling
    {
        get
        {
            string value = _config[$"{BulkProcessingKey}:{EnableMemoryThrottlingKey}"];
            return value.IzNullOrWhiteSpace() || bool.Parse(value);
        }
    }

    public int DashboardRefreshFrequency
    {
        get
        {
            string value = _config[$"{BulkProcessingKey}:{DashboardRefreshFrequencyKey}"];
            return value.IzNullOrWhiteSpace() ? 100 : int.Parse(value);
        }
    }

    public bool ProcessRulings
    {
        get
        {
            string value = _config[$"{BulkProcessingKey}:{ProcessRulingsKey}"];
            return value.IzNotNullOrWhiteSpace() && bool.Parse(value);
        }
    }

    public bool SetsOnly
    {
        get
        {
            string value = _config[$"{BulkProcessingKey}:{SetsOnlyKey}"];
            return value.IzNotNullOrWhiteSpace() && bool.Parse(value);
        }
    }

    public IReadOnlyCollection<string> SetCodesToProcess
    {
        get
        {
            string value = _config[$"{BulkProcessingKey}:{SetCodesToProcessKey}"];
            if (value.IzNullOrWhiteSpace()) return new List<string>();

            return value.Split(',')
                .Select(s => s.Trim().ToLowerInvariant())
                .Where(s => s.IzNotNullOrWhiteSpace())
                .ToList();
        }
    }
}