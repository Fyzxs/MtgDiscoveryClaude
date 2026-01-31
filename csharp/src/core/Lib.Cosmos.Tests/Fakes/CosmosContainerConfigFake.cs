using Lib.Cosmos.Apis.Configurations;

namespace Lib.Cosmos.Tests.Fakes;

public sealed class CosmosContainerConfigFake : ICosmosContainerConfig
{
    public CosmosAutoscaleMax AutoscaleMaxResult { get; init; }
    public int AutoscaleMaxInvokeCount { get; private set; }

    public CosmosContainerTimeToLive TimeToLiveResult { get; init; }
    public int TimeToLiveInvokeCount { get; private set; }

    public CosmosAutoscaleMax AutoscaleMax()
    {
        AutoscaleMaxInvokeCount++;
        return AutoscaleMaxResult;
    }

    public CosmosContainerTimeToLive TimeToLive()
    {
        TimeToLiveInvokeCount++;
        return TimeToLiveResult;
    }
}
