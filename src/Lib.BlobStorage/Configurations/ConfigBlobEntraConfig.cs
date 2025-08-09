using Lib.BlobStorage.Apis.Configurations;
using Lib.Universal.Configurations;

namespace Lib.BlobStorage.Configurations;

internal sealed class ConfigBlobEntraConfig : IBlobEntraConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigBlobEntraConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }
    public BlobAccountEndpoint AccountEndpoint()
    {
        return new ConfigBlobAccountEndpoint($"{_parentKey}:{IBlobEntraConfig.AccountEndpointKey}", _config);
    }
}
