using System;
using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosThroughputMode : ICosmosThroughputMode
{
    private const string DatabaseMode = "database";
    private const string ContainerMode = "container";

    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigCosmosThroughputMode(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public bool IsDatabaseShared()
    {
        string value = _config[_sourceKey];

        if (value.IzNullOrWhiteSpace() || value.Equals(ContainerMode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (value.Equals(DatabaseMode, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        throw new CosmosConfigurationException($"Invalid throughput_mode value [{value}]. Valid values are: {DatabaseMode}, {ContainerMode}");
    }

    public bool IsContainerDedicated() => IsDatabaseShared() is false;
}
