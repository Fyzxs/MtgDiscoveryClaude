namespace Lib.BlobStorage.Apis.Configurations;

/// <summary>
/// Defines the connection configuration for Azure Active Directory (Entra) authentication.
/// </summary>
public interface IBlobEntraConfig
{
    /// <summary>
    /// Configuration key for the account endpoint.
    /// </summary>
    const string AccountEndpointKey = "account_endpoint";

    /// <summary>
    /// Gets the account endpoint.
    /// </summary>
    /// <returns>The account endpoint.</returns>
    BlobAccountEndpoint AccountEndpoint();
}
