using Lib.BlobStorage.Apis.Configurations;
using Lib.Universal.Configurations;

namespace Lib.BlobStorage.Configurations;

internal sealed class ConfigBlobSasConfig : IBlobSasConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigBlobSasConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }
    public BlobConnectionString ConnectionString()
    {
        return new ConfigBlobConnectionString($"{_parentKey}:{IBlobSasConfig.ConnectionStringKey}", _config);
    }
}
