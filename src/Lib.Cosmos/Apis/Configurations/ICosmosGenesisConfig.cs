namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Defines common genesis configuration for Azure Cosmos DB authentication.
/// </summary>
public interface ICosmosGenesisConfig
{
    /// <summary>
    /// Configuration key for the bypass setting.
    /// </summary>
    const string BypassKey = "bypass";

    /// <summary>
    /// Gets whether to bypass genesis configuration.
    /// </summary>
    /// <returns>True if bypass is enabled; otherwise, false.</returns>
    bool Bypass();
}
