using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Lib.BlobStorage.Apis.Ids;
using Lib.BlobStorage.Apis.Operations.Responses;

namespace Lib.BlobStorage.Apis.Operations;

/// <summary>
/// Interface for a scribe that handles blob write operations.
/// </summary>
public interface IBlobWriteScribe
{
    /// <summary>
    /// Writes a blob to the storage container.
    /// </summary>
    /// <param name="blobWriteEntity">The entity containing blob write details.</param>
    /// <returns>A response indicating the result of the write operation.</returns>
    Task<BlobOpResponse<BlobContentInfo>> WriteAsync(IBlobBinaryWriteDomain blobWriteEntity);
}
