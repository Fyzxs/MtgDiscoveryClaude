using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Tests.Fakes;

public class CosmosContainerNameFake : CosmosContainerName
{
    private readonly string _value;

    public CosmosContainerNameFake(string value) => _value = value;

    public override string AsSystemType() => _value;
}