using System;
using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Cosmos.Configurations;

internal sealed class ConfigCosmosClientAuthMode : CosmosClientAuthMode
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigCosmosClientAuthMode(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override CosmosAuthMode AsSystemType()
    {
        string value = _config[_sourceKey];
        return "KeyAuth".Equals(value, StringComparison.CurrentCultureIgnoreCase)
            ? CosmosAuthMode.KeyAuth
            : CosmosAuthMode.EntraAuth;
    }
}
