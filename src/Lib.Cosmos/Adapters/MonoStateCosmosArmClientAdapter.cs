using System.Collections.Concurrent;
using Azure.Core;
using Azure.ResourceManager.Resources;
using Lib.Cosmos.Apis.Configurations;

namespace Lib.Cosmos.Adapters;

internal sealed class MonoStateCosmosArmClientAdapter : ICosmosArmClientAdapter
{
    private static readonly ConcurrentDictionary<string, ICosmosArmClientAdapter> s_cache = [];

    private readonly ICosmosContainerDefinition _containerDefinition;
    private readonly ICosmosConnectionConvenience _connectionConvenience;

    public MonoStateCosmosArmClientAdapter(ICosmosContainerDefinition containerDefinition, ICosmosConnectionConvenience connectionConvenience)
    {
        _containerDefinition = containerDefinition;
        _connectionConvenience = connectionConvenience;
    }

    private ICosmosArmClientAdapter MonoState()
    {
        string key = $"{_containerDefinition.FriendlyAccountName()}_{_containerDefinition.ContainerName()}";
        return s_cache.GetOrAdd(key, _ => new CosmosArmClientAdapter(_containerDefinition, _connectionConvenience));
    }

    public SubscriptionResource GetSubscriptionResource(ResourceIdentifier createResourceIdentifier) => MonoState().GetSubscriptionResource(createResourceIdentifier);
}
