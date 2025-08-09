using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.BlobStorage.Operations;

namespace Lib.BlobStorage.Apis.Operations;

/// <summary>
/// Defines operations for listing files and directories in a Blob container.
/// </summary>
public interface IBlobContainerListOperator
{
    /// <summary>
    /// Lists blob files
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of file.</returns>
    Task<IList<IBlobListingItem>> BlobListAsync();
}
