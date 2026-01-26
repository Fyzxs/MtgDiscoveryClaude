using Lib.Cosmos.Apis.Configurations;

namespace Lib.Cosmos.Tests.Fakes;

internal sealed class CosmosThroughputModeFake : ICosmosThroughputMode
{
    public bool IsDatabaseSharedResult { get; init; }
    public int IsDatabaseSharedInvokeCount { get; private set; }

    public int IsContainerDedicatedInvokeCount { get; private set; }

    public bool IsDatabaseShared()
    {
        IsDatabaseSharedInvokeCount++;
        return IsDatabaseSharedResult;
    }

    public bool IsContainerDedicated()
    {
        IsContainerDedicatedInvokeCount++;
        return IsDatabaseSharedResult is false;
    }
}
