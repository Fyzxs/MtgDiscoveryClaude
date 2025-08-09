using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Tests.Fakes;

public class CosmosContainerDefinitionFake : ICosmosContainerDefinition
{
    public CosmosAccountName AccountNameResult { get; init; }
    public int AccountNameInvokeCount { get; private set; }

    public CosmosDatabaseName DatabaseNameResult { get; init; }
    public int DatabaseNameInvokeCount { get; private set; }

    public CosmosContainerName ContainerNameResult { get; init; }
    public int ContainerNameInvokeCount { get; private set; }

    public CosmosPartitionKeyPath PartitionKeyPathResult { get; init; }
    public int PartitionKeyPathInvokeCount { get; private set; }

    public CosmosFriendlyAccountName FriendlyAccountNameResult { get; init; }
    public int FriendlyAccountNameInvokeCount { get; private set; }

    public CosmosAccountName AccountName()
    {
        AccountNameInvokeCount++;
        return AccountNameResult;
    }

    public CosmosFriendlyAccountName FriendlyAccountName()
    {
        FriendlyAccountNameInvokeCount++;
        return FriendlyAccountNameResult;
    }

    public CosmosDatabaseName DatabaseName()
    {
        DatabaseNameInvokeCount++;
        return DatabaseNameResult;
    }

    public CosmosContainerName ContainerName()
    {
        ContainerNameInvokeCount++;
        return ContainerNameResult;
    }

    public CosmosPartitionKeyPath PartitionKeyPath()
    {
        PartitionKeyPathInvokeCount++;
        return PartitionKeyPathResult;
    }
}
