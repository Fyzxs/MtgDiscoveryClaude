using Lib.Cosmos.Apis.Configurations;

namespace Lib.Cosmos.Tests.Fakes;

internal sealed class CosmosDatabaseConfigFake : ICosmosDatabaseConfig
{
    public ICosmosContainerConfig ContainerConfigResult { get; init; }
    public int ContainerConfigInvokeCount { get; private set; }

    public ICosmosThroughputMode ThroughputModeResult { get; init; }
    public int ThroughputModeInvokeCount { get; private set; }

    public CosmosAutoscaleMax AutoscaleMaxResult { get; init; }
    public int AutoscaleMaxInvokeCount { get; private set; }

    public ICosmosContainerConfig ContainerConfig(ICosmosContainerDefinition cosmosContainerDefinition)
    {
        ContainerConfigInvokeCount++;
        return ContainerConfigResult;
    }

    public ICosmosThroughputMode ThroughputMode()
    {
        ThroughputModeInvokeCount++;
        return ThroughputModeResult;
    }

    public CosmosAutoscaleMax AutoscaleMax()
    {
        AutoscaleMaxInvokeCount++;
        return AutoscaleMaxResult;
    }
}
