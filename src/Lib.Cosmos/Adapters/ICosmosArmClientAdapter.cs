using Azure.Core;
using Azure.ResourceManager.Resources;

namespace Lib.Cosmos.Adapters;

internal interface ICosmosArmClientAdapter
{
    SubscriptionResource GetSubscriptionResource(ResourceIdentifier createResourceIdentifier);
}
