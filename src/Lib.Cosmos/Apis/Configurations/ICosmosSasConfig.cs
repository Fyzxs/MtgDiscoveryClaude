namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Defines the configuration for Shared Access Signature (SAS) authentication.
/// </summary>
public interface ICosmosSasConfig
{

    /// <summary>
    /// Configuration key for the connection section.
    /// </summary>
    const string ConnectionKey = "connection";

    /// <summary>
    /// Gets the connection configuration.
    /// </summary>
    /// <returns>The connection configuration.</returns>
    ICosmosSasConnectionConfig ConnectionConfig();
}
