namespace Lib.Cosmos.Apis.Configurations;

/// <summary>
/// Defines the genesis configuration for Azure Active Directory (Entra) authentication.
/// </summary>
public interface ICosmosEntraGenesisConfig : ICosmosGenesisConfig
{
    /// <summary>
    /// Configuration key for the subscription ID.
    /// </summary>
    const string SubscriptionIdKey = "subscription_id";

    /// <summary>
    /// Configuration key for the resource group name.
    /// </summary>
    const string ResourceGroupNameKey = "resource_group_name";

    /// <summary>
    /// Gets the subscription ID.
    /// </summary>
    /// <returns>The subscription ID.</returns>
    string SubscriptionId();

    /// <summary>
    /// Gets the resource group name.
    /// </summary>
    /// <returns>The resource group name.</returns>
    string ResourceGroupName();
}
