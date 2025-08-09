using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Configurations.Ids;
using Lib.Universal.Configurations;

namespace Lib.BlobStorage.Configurations;

internal sealed class ConfigBlobContainerConfig : IBlobContainerConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigBlobContainerConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }

    public BlobContainerName Name()
    {
        return new ConfigBlobContainerName($"{_parentKey}:{IBlobContainerConfig.ContainerNameKey}", _config);
    }

    public BlobContainerPublicAccessType AccessType()
    {
        return new ConfigBlobContainerPublicAccessType($"{_parentKey}:{IBlobContainerConfig.PublicAccessKey}", _config);
    }
}
