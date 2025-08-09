using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Lib.BlobStorage.Apis.Ids;
using Lib.BlobStorage.Apis.Operations.Responses;

namespace Lib.BlobStorage.Apis.Operations;

/// <inheritdoc />
public abstract class BlobWriteScribe : IBlobWriteScribe
{
    private readonly IBlobContainerWriteOperator _source;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlobWriteScribe"/> class.
    /// </summary>
    /// <param name="source">The container write operator to delegate operations to.</param>
    protected BlobWriteScribe(IBlobContainerWriteOperator source) => _source = source;

    /// <inheritdoc />
    public async Task<BlobOpResponse<BlobContentInfo>> WriteAsync(IBlobBinaryWriteDomain blobWriteEntity) => await _source.WriteAsync(blobWriteEntity).ConfigureAwait(false);
}
