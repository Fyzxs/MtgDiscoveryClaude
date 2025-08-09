using System.Threading.Tasks;
using Lib.BlobStorage.Apis.Ids;

namespace Lib.BlobStorage.Apis.Operations;

/// <inheritdoc />
public abstract class BlobJanitor : IBlobJanitor
{
    private readonly IBlobContainerDeleteOperator _source;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlobJanitor"/> class.
    /// </summary>
    /// <param name="source">The container read operator to delegate operations to.</param>
    protected BlobJanitor(IBlobContainerDeleteOperator source) => _source = source;

    /// <inheritdoc />
    public async Task DeleteAsync(BlobPathEntity blobItemPath) => await _source.DeleteAsync(blobItemPath).ConfigureAwait(false);
}
