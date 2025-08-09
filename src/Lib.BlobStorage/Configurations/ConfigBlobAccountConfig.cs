using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Configurations.Ids;
using Lib.Universal.Configurations;

namespace Lib.BlobStorage.Configurations;

internal sealed class ConfigBlobAccountConfig : IBlobAccountConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigBlobAccountConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }

    public BlobClientAuthMode AuthMode()
    {
        return new ConfigBlobClientAuthMode($"{_parentKey}:{IBlobAccountConfig.AuthModeKey}", _config);
    }

    public BlobAccountName AccountName(IBlobContainerDefinition blobContainerDefinition)
    {
        return new ConfigBlobAccountName($"{_parentKey}:{IBlobAccountConfig.AccountNameKey}", _config);
    }

    public IBlobContainerConfig ContainerConfig(IBlobContainerDefinition blobContainerDefinition)
    {
        return new ConfigBlobContainerConfig($"{_parentKey}:{blobContainerDefinition.FriendlyContainerName().AsSystemType()}", _config);
    }

    public IBlobEntraConfig EntraConfig()
    {
        return new ConfigBlobEntraConfig($"{_parentKey}:{IBlobAccountConfig.EntraKey}", _config);
    }

    public IBlobSasConfig SasConfig()
    {
        return new ConfigBlobSasConfig($"{_parentKey}:{IBlobAccountConfig.SasKey}", _config);
    }
}
