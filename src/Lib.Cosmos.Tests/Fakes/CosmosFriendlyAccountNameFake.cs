using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Tests.Fakes;

public class CosmosFriendlyAccountNameFake : CosmosFriendlyAccountName
{
    private readonly string _value;

    public CosmosFriendlyAccountNameFake(string value) => _value = value;

    public override string AsSystemType() => _value;
}