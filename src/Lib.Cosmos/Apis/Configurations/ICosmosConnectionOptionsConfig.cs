namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Defines common connection options for Azure Cosmos DB.
/// </summary>
public interface ICosmosConnectionOptionsConfig
{
    /// <summary>
    /// Configuration key for the preferred regions.
    /// </summary>
    const string PreferredRegionsKey = "preferred_regions";

    /// <summary>
    /// Configuration key for the connection mode.
    /// </summary>
    const string ConnectionModeKey = "connection_mode";

    /// <summary>
    /// Gets the preferred regions.
    /// </summary>
    /// <returns>The preferred regions.</returns>
    CosmosPreferredRegions PreferredRegions();

    /// <summary>
    /// Gets the connection mode.
    /// </summary>
    /// <returns>The connection mode.</returns>
    CosmosConnectionMode ConnectionMode();
}