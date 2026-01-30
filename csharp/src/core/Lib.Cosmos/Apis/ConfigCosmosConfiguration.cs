using System.Diagnostics.CodeAnalysis;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Configurations;
using Lib.Universal.Configurations;

namespace Lib.Cosmos.Apis;

/// <summary>
/// Provides configuration for Azure Cosmos DB using the configuration system.
/// This implementation retrieves Cosmos DB settings from the application configuration.
/// </summary>
public sealed class ConfigCosmosConfiguration : ICosmosConfiguration
{
    /// <summary>
    /// The configuration provider used to retrieve Cosmos DB settings.
    /// </summary>
    private readonly IConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigCosmosConfiguration"/> class
    /// using the default mono-state configuration.
    /// </summary>
    public ConfigCosmosConfiguration() : this(new MonoStateConfig())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigCosmosConfiguration"/> class
    /// with the specified configuration provider.
    /// </summary>
    /// <param name="config">The configuration provider to use for retrieving Cosmos DB settings.</param>
    private ConfigCosmosConfiguration(IConfig config) => _config = config;

    /// <summary>
    /// Gets the account configuration for the specified container definition.
    /// </summary>
    /// <param name="cosmosContainerDefinition">The container definition that specifies the account.</param>
    /// <returns>The account configuration for the specified container's account.</returns>
    public ICosmosAccountConfig AccountConfig([NotNull] ICosmosContainerDefinition cosmosContainerDefinition)
    {
        string accountKey = $"{ICosmosConfiguration.CerberusCosmosConfigKey}:{cosmosContainerDefinition.FriendlyAccountName().AsSystemType()}";
        return new ConfigCosmosAccountConfig(accountKey, _config);
    }
}
