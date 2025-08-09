using Azure.Storage.Blobs.Models;
using Lib.BlobStorage.Apis.Configurations;
using Lib.Universal.Configurations;
using Lib.Universal.Extensions;

namespace Lib.BlobStorage.Configurations;

internal sealed class ConfigBlobContainerPublicAccessType : BlobContainerPublicAccessType
{
    private readonly string _sourceKey;
    private readonly IConfig _config;

    public ConfigBlobContainerPublicAccessType(string sourceKey, IConfig config)
    {
        _sourceKey = sourceKey;
        _config = config;
    }

    public override PublicAccessType AsSystemType()
    {
        string value = _config[_sourceKey];
        if (value.IzNullOrWhiteSpace()) throw new BlobConfigurationException($"{GetType().Name} requires key [{_sourceKey}]");
        return value.ToLowerInvariant() switch
        {
            "blob" => PublicAccessType.Blob,
            _ => PublicAccessType.None
        };
    }
}
