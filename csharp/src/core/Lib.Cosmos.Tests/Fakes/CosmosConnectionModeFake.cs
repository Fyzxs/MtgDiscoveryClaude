using Lib.Cosmos.Apis.Configurations;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Tests.Fakes;

public sealed class CosmosConnectionModeFake : CosmosConnectionMode
{
    private readonly ConnectionMode _connectionMode;

    public CosmosConnectionModeFake(ConnectionMode connectionMode) => _connectionMode = connectionMode;

    public override ConnectionMode AsSystemType() => _connectionMode;
}
