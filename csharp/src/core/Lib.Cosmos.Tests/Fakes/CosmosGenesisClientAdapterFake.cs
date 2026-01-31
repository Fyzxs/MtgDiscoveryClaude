using System.Threading.Tasks;
using Lib.Cosmos.Adapters;
using Lib.Cosmos.Apis.Configurations;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Tests.Fakes;

public sealed class CosmosGenesisClientAdapterFake : ICosmosGenesisClientAdapter
{
    public Container GetContainerResult { get; init; }
    public int GetContainerInvokeCount { get; private set; }

    public DatabaseResponse CreateDatabaseIfNotExistsAsyncResult { get; init; }
    public int CreateDatabaseIfNotExistsAsyncInvokeCount { get; private set; }
    public ThroughputProperties CapturedThroughputProperties { get; private set; }

    public Task<Container> GetContainer(ICosmosContainerDefinition cosmosContainerDef)
    {
        GetContainerInvokeCount++;
        return Task.FromResult(GetContainerResult);
    }

    public Task<DatabaseResponse> CreateDatabaseIfNotExistsAsync(ICosmosContainerDefinition containerDefinition, ThroughputProperties throughputProperties)
    {
        CreateDatabaseIfNotExistsAsyncInvokeCount++;
        CapturedThroughputProperties = throughputProperties;
        return Task.FromResult(CreateDatabaseIfNotExistsAsyncResult);
    }
}
