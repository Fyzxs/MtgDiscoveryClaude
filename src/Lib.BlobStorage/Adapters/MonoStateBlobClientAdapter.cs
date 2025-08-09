using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Lib.BlobStorage.Apis.Configurations;

namespace Lib.BlobStorage.Adapters;

internal sealed class MonoStateBlobClientAdapter : IBlobServiceClientAdapter
{
    private static readonly Dictionary<string, BlobContainerClient> s_cache = [];
    private static readonly SemaphoreSlim s_cacheLock = new(1, 1);

    private readonly IBlobContainerDefinition _containerDefinition;
    private readonly IBlobServiceClientAdapter _clientAdapter;

    public MonoStateBlobClientAdapter(IBlobContainerDefinition containerDefinition, IBlobConnectionConvenience connectionConvenience)
        : this(containerDefinition, new BlobServiceClientAdapter(containerDefinition, connectionConvenience))
    { }

    private MonoStateBlobClientAdapter(IBlobContainerDefinition containerDefinition, IBlobServiceClientAdapter clientAdapter)
    {
        _containerDefinition = containerDefinition;
        _clientAdapter = clientAdapter;
    }

    private async Task<BlobContainerClient> MonoStateAsync()
    {
        string key = $"{_containerDefinition.FriendlyAccountName().AsSystemType()}_{_containerDefinition.FriendlyContainerName().AsSystemType()}";

        await s_cacheLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (s_cache.TryGetValue(key, out BlobContainerClient blobContainerClient)) return blobContainerClient;
            blobContainerClient = await _clientAdapter.BlobContainerClient().ConfigureAwait(false);
            s_cache[key] = blobContainerClient;
            return blobContainerClient;
        }
        finally
        {
            s_cacheLock.Release();
        }
    }

    public async Task<BlobContainerClient> BlobContainerClient()
    {
        return await MonoStateAsync().ConfigureAwait(false);
    }
}
