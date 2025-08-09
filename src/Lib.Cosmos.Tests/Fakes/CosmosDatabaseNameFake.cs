using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Tests.Fakes;

public class CosmosDatabaseNameFake : CosmosDatabaseName
{
    private readonly string _value;

    public CosmosDatabaseNameFake(string value) => _value = value;

    public override string AsSystemType() => _value;
}