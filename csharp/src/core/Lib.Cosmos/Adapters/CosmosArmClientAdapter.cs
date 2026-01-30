using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Lib.Cosmos.Apis.Configurations;

namespace Lib.Cosmos.Adapters;

internal sealed class CosmosArmClientAdapter : ICosmosArmClientAdapter
{
    private readonly ArmClient _armClient;

    public CosmosArmClientAdapter(ICosmosContainerDefinition containerDefinition, ICosmosConnectionConvenience connectionConvenience) : this(new ArmClient(connectionConvenience.AccountEntraCredential(containerDefinition)))
    { }

    private CosmosArmClientAdapter(ArmClient armClient) => _armClient = armClient;

    public SubscriptionResource GetSubscriptionResource(ResourceIdentifier createResourceIdentifier) => _armClient.GetSubscriptionResource(createResourceIdentifier);
}
