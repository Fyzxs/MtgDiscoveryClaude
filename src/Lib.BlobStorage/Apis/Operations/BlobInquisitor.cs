using System.Threading.Tasks;
using Lib.BlobStorage.Apis.Ids;
using Lib.BlobStorage.Apis.Operations.Responses;

namespace Lib.BlobStorage.Apis.Operations;

/// <inheritdoc />
public abstract class BlobInquisitor : IBlobInquisitor
{
    private readonly IBlobContainerExistsOperator _source;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlobInquisitor"/> class.
    /// </summary>
    /// <param name="source">The container read operator to delegate operations to.</param>
    protected BlobInquisitor(IBlobContainerExistsOperator source) => _source = source;

    /// <inheritdoc />
    public async Task<BlobOpResponse<bool>> ExistsAsync(BlobPathEntity blobItemPath) => await _source.ExistsAsync(blobItemPath).ConfigureAwait(false);
}
