using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Tests.Fakes;

internal sealed class DatabaseResponseFake : DatabaseResponse
{
    public Database DatabaseResult { get; init; }

    public override Database Database => DatabaseResult;
}
