using Lib.Cosmos.Apis.Configurations;

namespace Lib.Cosmos.Tests.Fakes;

internal sealed class CosmosConnectionOptionsConfigFake : ICosmosConnectionOptionsConfig
{
    public CosmosConnectionMode ConnectionModeResult { get; init; }
    public CosmosPreferredRegions PreferredRegionsResult { get; init; }

    public int ConnectionModeInvokeCount { get; private set; }
    public int PreferredRegionsInvokeCount { get; private set; }

    public CosmosConnectionMode ConnectionMode()
    {
        ConnectionModeInvokeCount++;
        return ConnectionModeResult;
    }

    public CosmosPreferredRegions PreferredRegions()
    {
        PreferredRegionsInvokeCount++;
        return PreferredRegionsResult;
    }
}