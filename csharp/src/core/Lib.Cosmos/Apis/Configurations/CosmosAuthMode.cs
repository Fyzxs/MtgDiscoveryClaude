namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Specifies the authentication mode for connecting to Azure Cosmos DB.
/// </summary>
public enum CosmosAuthMode
{
    /// <summary>
    /// Key-based authentication using account keys.
    /// </summary>
    KeyAuth = 0,

    /// <summary>
    /// Azure Active Directory (AAD) authentication.
    /// </summary>
    EntraAuth = 1
}
