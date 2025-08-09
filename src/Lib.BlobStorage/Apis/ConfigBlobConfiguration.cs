using System.Diagnostics.CodeAnalysis;
using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Configurations;
using Lib.Universal.Configurations;

namespace Lib.BlobStorage.Apis;

/// <summary>
/// Provides configuration for Azure Blob DB using the configuration system.
/// This implementation retrieves Blob DB settings from the application configuration.
/// </summary>
public sealed class ConfigBlobConfiguration : IBlobConfiguration
{
    /// <summary>
    /// The configuration provider used to retrieve Blob DB settings.
    /// </summary>
    private readonly IConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigBlobConfiguration"/> class
    /// using the default mono-state configuration.
    /// </summary>
    public ConfigBlobConfiguration() : this(new MonoStateConfig())
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigBlobConfiguration"/> class
    /// with the specified configuration provider.
    /// </summary>
    /// <param name="config">The configuration provider to use for retrieving Blob DB settings.</param>
    private ConfigBlobConfiguration(IConfig config) => _config = config;

    /// <summary>
    /// Gets the account configuration for the specified container definition.
    /// </summary>
    /// <param name="blobContainerDefinition">The container definition that specifies the account.</param>
    /// <returns>The account configuration for the specified container's account.</returns>
    public IBlobAccountConfig AccountConfig([NotNull] IBlobContainerDefinition blobContainerDefinition)
    {
        string accountKey = $"{IBlobConfiguration.CerberusBlobConfigKey}:{blobContainerDefinition.FriendlyAccountName().AsSystemType()}";
        return new ConfigBlobAccountConfig(accountKey, _config);
    }
}
