using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Configurations.Ids;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.BlobStorage.Configurations;

internal sealed class ConfigBlobAccountName : BlobAccountName
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigBlobAccountName(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override string AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new BlobConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        return value;
    }
}
