using System;
using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosConnectionMode : CosmosConnectionMode
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigCosmosConnectionMode(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override ConnectionMode AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new CosmosConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        return "gateway".Equals(value, StringComparison.CurrentCultureIgnoreCase)
            ? ConnectionMode.Gateway
            : ConnectionMode.Direct;
    }
}
