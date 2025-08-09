using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Defines the configuration for an Azure Cosmos DB account.
/// </summary>
public interface ICosmosAccountConfig
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
    CosmosClientAuthMode AuthMode();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    CosmosAccountName AccountName();

    /// <summary>
    /// Gets the database configuration for the specified container configuration.
    /// </summary>
    /// <param name="cosmosContainerDefinition">The container configuration.</param>
    /// <returns>The database configuration.</returns>
    ICosmosDatabaseConfig DatabaseConfig(ICosmosContainerDefinition cosmosContainerDefinition);

    /// <summary>
    /// Gets the Entra (Azure Active Directory) configuration.
    /// </summary>
    /// <returns>The Entra configuration.</returns>
    ICosmosEntraConfig EntraConfig();

    /// <summary>
    /// Gets the SAS (Shared Access Signature) configuration.
    /// </summary>
    /// <returns>The SAS configuration.</returns>
    ICosmosSasConfig SasConfig();
}
