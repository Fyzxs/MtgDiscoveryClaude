using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Lib.BlobStorage.Apis.Ids;
using Lib.BlobStorage.Apis.Operations.Responses;

namespace Lib.BlobStorage.Apis.Operations;

/// <summary>
/// 
/// </summary>
public interface IBlobContainerWriteOperator
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="blobWriteEntity"></param>
    /// <returns></returns>
    Task<BlobOpResponse<BlobContentInfo>> WriteAsync(IBlobBinaryWriteDomain blobWriteEntity);
}
