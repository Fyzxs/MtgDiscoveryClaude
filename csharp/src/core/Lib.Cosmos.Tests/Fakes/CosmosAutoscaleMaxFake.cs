using Lib.Cosmos.Apis.Configurations;

namespace Lib.Cosmos.Tests.Fakes;

public sealed class CosmosAutoscaleMaxFake : CosmosAutoscaleMax
{
    private readonly int _value;

    public CosmosAutoscaleMaxFake(int value) => _value = value;

    public override int AsSystemType() => _value;
}
