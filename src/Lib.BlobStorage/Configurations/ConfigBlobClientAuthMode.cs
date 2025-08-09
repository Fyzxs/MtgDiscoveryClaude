using System;
using Lib.BlobStorage.Apis.Configurations;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.BlobStorage.Configurations;

internal sealed class ConfigBlobClientAuthMode : BlobClientAuthMode
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigBlobClientAuthMode(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override BlobAuthMode AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new BlobConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        return "KeyAuth".Equals(value, StringComparison.CurrentCultureIgnoreCase)
            ? BlobAuthMode.KeyAuth
            : BlobAuthMode.EntraAuth;
    }
}
