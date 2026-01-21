using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

namespace Lib.Cosmos.Tests.Fakes;

internal sealed class ArmClientFake : ArmClient
{
    public SubscriptionResource GetSubscriptionResourceResult { get; init; }
    public int GetSubscriptionResourceInvokeCount { get; private set; }

    public override SubscriptionResource GetSubscriptionResource(ResourceIdentifier id)
    {
        GetSubscriptionResourceInvokeCount++;
        return GetSubscriptionResourceResult;
    }
}
