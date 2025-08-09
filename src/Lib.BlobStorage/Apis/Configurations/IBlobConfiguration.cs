namespace Lib.BlobStorage.Apis.Configurations;

/// <summary>
/// Defines the root configuration for Azure Blob DB.
/// </summary>
public interface IBlobConfiguration
{
    /// <summary>
    /// Configuration key for the Blob configuration section.
    /// </summary>
    const string CerberusBlobConfigKey = "CerberusBlobConfig";

    /// <summary>
    /// Gets the account configuration for the specified container configuration.
    /// </summary>
    /// <param name="blobContainerDefinition">The container configuration.</param>
    /// <returns>The account configuration.</returns>
    IBlobAccountConfig AccountConfig(IBlobContainerDefinition blobContainerDefinition);
}
