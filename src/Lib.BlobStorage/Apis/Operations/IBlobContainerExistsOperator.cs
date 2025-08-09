using System.Threading.Tasks;
using Lib.BlobStorage.Apis.Ids;
using Lib.BlobStorage.Apis.Operations.Responses;

namespace Lib.BlobStorage.Apis.Operations;

/// <summary>
/// 
/// </summary>
public interface IBlobContainerExistsOperator
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="blobItemPath"></param>
    /// <returns></returns>
    Task<BlobOpResponse<bool>> ExistsAsync(BlobPathEntity blobItemPath);
}
