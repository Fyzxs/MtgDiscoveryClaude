using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Lib.BlobStorage.Apis.Configurations;

namespace Lib.BlobStorage.Adapters;

internal sealed class BlobServiceClientAdapter : IBlobServiceClientAdapter
{
    private readonly IBlobContainerDefinition _containerDefinition;
    private readonly IBlobConnectionConvenience _connectionConvenience;

    public BlobServiceClientAdapter(IBlobContainerDefinition containerDefinition, IBlobConnectionConvenience connectionConvenience)
    {
        _containerDefinition = containerDefinition;
        _connectionConvenience = connectionConvenience;
    }

    private BlobServiceClient Adapter()
    {
        BlobServiceClient adapter = BlobAuthMode.KeyAuth.Equals(_connectionConvenience.AccountConfig(_containerDefinition).AuthMode().AsSystemType())
            ? new BlobServiceClient(_connectionConvenience.AccountConfig(_containerDefinition).SasConfig().ConnectionString())
            : new BlobServiceClient(_connectionConvenience.AccountConfig(_containerDefinition).EntraConfig().AccountEndpoint(),
                _connectionConvenience.AccountEntraCredential(_containerDefinition));
        return adapter;
    }

    public async Task<BlobContainerClient> BlobContainerClient()
    {
        BlobServiceClient blobServiceClient = Adapter();
        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(_connectionConvenience.ContainerConfig(_containerDefinition).Name());
        await Task.Delay(0).ConfigureAwait(false);
        return blobContainerClient;
    }
}
