using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Tests.Fakes;

public class CosmosPartitionKeyPathFake : CosmosPartitionKeyPath
{
    private readonly string _value;

    public CosmosPartitionKeyPathFake(string value) => _value = value;

    public override string AsSystemType() => _value;
}
