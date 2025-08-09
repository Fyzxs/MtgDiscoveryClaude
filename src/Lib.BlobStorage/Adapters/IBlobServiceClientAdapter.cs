using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace Lib.BlobStorage.Adapters;

internal interface IBlobServiceClientAdapter
{
    Task<BlobContainerClient> BlobContainerClient();
}
