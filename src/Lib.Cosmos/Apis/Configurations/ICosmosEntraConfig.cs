namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Defines the configuration for Azure Active Directory (Entra) authentication.
/// </summary>
public interface ICosmosEntraConfig
{
    /// <summary>
    /// Configuration key for the genesis section.
    /// </summary>
    const string GenesisKey = "genesis";

    /// <summary>
    /// Configuration key for the connection section.
    /// </summary>
    const string ConnectionKey = "connection";

    /// <summary>
    /// Gets the genesis configuration.
    /// </summary>
    /// <returns>The genesis configuration.</returns>
    ICosmosEntraGenesisConfig GenesisConfig();

    /// <summary>
    /// Gets the connection configuration.
    /// </summary>
    /// <returns>The connection configuration.</returns>
    ICosmosEntraConnectionConfig ConnectionConfig();
}