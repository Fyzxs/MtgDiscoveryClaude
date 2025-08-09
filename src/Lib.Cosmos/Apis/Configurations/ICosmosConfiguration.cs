namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Defines the root configuration for Azure Cosmos DB.
/// </summary>
public interface ICosmosConfiguration
{
    /// <summary>
    /// Configuration key for the Cosmos configuration section.
    /// </summary>
    const string CerberusCosmosConfigKey = "CerberusCosmosConfig";

    /// <summary>
    /// Gets the account configuration for the specified container configuration.
    /// </summary>
    /// <param name="cosmosContainerDefinition">The container configuration.</param>
    /// <returns>The account configuration.</returns>
    ICosmosAccountConfig AccountConfig(ICosmosContainerDefinition cosmosContainerDefinition);
}
