using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.BlobStorage.Operations;

namespace Lib.BlobStorage.Apis.Operations;

/// <inheritdoc />
public abstract class BlobListMaker : IBlobListMaker
{
    private readonly IBlobContainerListOperator _source;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    protected BlobListMaker(IBlobContainerListOperator source) => _source = source;

    /// <inheritdoc />
    public async Task<IList<IBlobListingItem>> BlobListAsync() => await _source.BlobListAsync().ConfigureAwait(false);
}
