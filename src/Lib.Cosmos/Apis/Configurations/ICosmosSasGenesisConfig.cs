namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Defines the genesis configuration for Shared Access Signature (SAS) authentication.
/// </summary>
public interface ICosmosSasGenesisConfig : ICosmosGenesisConfig
{
    /// <summary>
    /// Configuration key for the connection string.
    /// </summary>
    const string ConnectionStringKey = "connectionstring";

    /// <summary>
    /// Gets the connection string.
    /// </summary>
    /// <returns>The connection string.</returns>
    CosmosConnectionString ConnectionString();
}