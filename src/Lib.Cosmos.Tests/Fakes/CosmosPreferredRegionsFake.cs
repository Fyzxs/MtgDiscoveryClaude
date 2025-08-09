using Lib.Cosmos.Apis.Configurations;

namespace Lib.Cosmos.Tests.Fakes;

internal sealed class CosmosPreferredRegionsFake : CosmosPreferredRegions
{
    private readonly string[] _regions;

    public CosmosPreferredRegionsFake(params string[] regions)
    {
        _regions = regions;
    }

    public override string[] AsSystemType() => _regions;
}
