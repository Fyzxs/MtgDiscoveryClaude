using System.Threading.Tasks;
using Lib.BlobStorage.Apis.Ids;

namespace Lib.BlobStorage.Apis.Operations;

/// <summary>
/// 
/// </summary>
public interface IBlobContainerDeleteOperator
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="blobItemPath"></param>
    /// <returns></returns>
    Task DeleteAsync(BlobPathEntity blobItemPath);
}
