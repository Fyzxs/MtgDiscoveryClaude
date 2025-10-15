using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Tests.Fakes;

internal sealed class CosmosClientFake : CosmosClient
{
    public Container GetContainerResult { get; init; }
    public int GetContainerInvokeCount { get; private set; }

    public Database GetDatabaseResult { get; init; }
    public int GetDatabaseInvokeCount { get; private set; }

    public DatabaseResponse CreateDatabaseIfNotExistsAsyncResult { get; init; }
    public int CreateDatabaseIfNotExistsAsyncInvokeCount { get; private set; }

    public override Container GetContainer(string databaseId, string containerId)
    {
        GetContainerInvokeCount++;
        return GetContainerResult;
    }

    public override Database GetDatabase(string id)
    {
        GetDatabaseInvokeCount++;
        return GetDatabaseResult;
    }

    public override Task<DatabaseResponse> CreateDatabaseIfNotExistsAsync(
        string id,
        ThroughputProperties throughputProperties,
        RequestOptions requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        CreateDatabaseIfNotExistsAsyncInvokeCount++;
        return Task.FromResult(CreateDatabaseIfNotExistsAsyncResult);
    }

    public override Task<DatabaseResponse> CreateDatabaseIfNotExistsAsync(
        string id,
        int? throughput = null,
        RequestOptions requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        CreateDatabaseIfNotExistsAsyncInvokeCount++;
        return Task.FromResult(CreateDatabaseIfNotExistsAsyncResult);
    }
#pragma warning disable CA2215
    protected override void Dispose(bool disposing) { }
#pragma warning restore CA2215
}
