using Lib.Cosmos.Apis.Configurations;

namespace Lib.Cosmos.Tests.Fakes;

internal sealed class CosmosContainerTimeToLiveFake : CosmosContainerTimeToLive
{
    private readonly int? _value;

    public CosmosContainerTimeToLiveFake(int? value) => _value = value;

    public override int? AsSystemType() => _value;
}
