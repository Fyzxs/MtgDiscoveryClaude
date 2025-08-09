using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Tests.Fakes;

public class CosmosAccountNameFake : CosmosAccountName
{
    private readonly string _value;

    public CosmosAccountNameFake(string value) => _value = value;

    public override string AsSystemType() => _value;
}
