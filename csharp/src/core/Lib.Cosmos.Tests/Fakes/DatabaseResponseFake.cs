using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Tests.Fakes;

public sealed class DatabaseResponseFake : DatabaseResponse
{
    public Database DatabaseResult { get; init; }

    public override Database Database => DatabaseResult;
}
