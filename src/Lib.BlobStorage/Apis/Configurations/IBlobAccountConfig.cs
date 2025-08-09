using Lib.BlobStorage.Apis.Configurations.Ids;

namespace Lib.BlobStorage.Apis.Configurations;

/// <summary>
/// Defines the configuration for an Azure Blob DB account.
/// </summary>
public interface IBlobAccountConfig
{
    /// <summary>
    /// Configuration key for the authentication mode.
    /// </summary>
    const string AuthModeKey = "auth_mode";

    /// <summary>
    /// Configuration key for the accountName.
    /// </summary>
    const string AccountNameKey = "account_name";

    /// <summary>
    /// Configuration key for the Entra configuration section.
    /// </summary>
    const string EntraKey = "entra";

    /// <summary>
    /// Configuration key for the SAS configuration section.
    /// </summary>
    const string SasKey = "sas";

    /// <summary>
    /// Gets the authentication mode for this account.
    /// </summary>
    /// <returns>The authentication mode.</returns>
    BlobClientAuthMode AuthMode();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    BlobAccountName AccountName(IBlobContainerDefinition blobContainerDefinition);

    /// <summary>
    /// Gets the database configuration for the specified container configuration.
    /// </summary>
    /// <param name="blobContainerDefinition">The container configuration.</param>
    /// <returns>The database configuration.</returns>
    IBlobContainerConfig ContainerConfig(IBlobContainerDefinition blobContainerDefinition);

    /// <summary>
    /// Gets the Entra (Azure Active Directory) configuration.
    /// </summary>
    /// <returns>The Entra configuration.</returns>
    IBlobEntraConfig EntraConfig();

    /// <summary>
    /// Gets the SAS (Shared Access Signature) configuration.
    /// </summary>
    /// <returns>The SAS configuration.</returns>
    IBlobSasConfig SasConfig();
}
