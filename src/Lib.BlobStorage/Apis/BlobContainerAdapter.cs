using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Lib.BlobStorage.Adapters;
using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Ids;
using Lib.BlobStorage.Apis.Operations;
using Lib.BlobStorage.Apis.Operations.Responses;
using Lib.BlobStorage.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.BlobStorage.Apis;

/// <summary>
/// 
/// </summary>
public abstract class BlobContainerAdapter : IBlobContainerAdapter
{
    private readonly IBlobContainerExistsOperator _exists;
    private readonly IBlobContainerDeleteOperator _delete;
    private readonly IBlobContainerWriteOperator _write;
    private readonly IBlobContainerListOperator _list;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="containerConfig"></param>
    /// <param name="connectionConfig"></param>
    protected BlobContainerAdapter(ILogger logger, IBlobContainerDefinition containerConfig, IBlobConnectionConvenience connectionConfig) : this(
        new BlobContainerExistsOperator(logger, containerConfig, connectionConfig),
        new BlobContainerDeleteOperator(logger, containerConfig, connectionConfig),
        new BlobContainerWriteOperator(logger, containerConfig, connectionConfig),
        new BlobContainerListOperator(logger, containerConfig, connectionConfig)
        )
    { }

    private BlobContainerAdapter(IBlobContainerExistsOperator exists,
        IBlobContainerDeleteOperator delete,
        IBlobContainerWriteOperator write,
        IBlobContainerListOperator list)
    {
        _exists = exists;
        _delete = delete;
        _write = write;
        _list = list;
    }

    /// <inheritdoc />
    public async Task<BlobOpResponse<bool>> ExistsAsync(BlobPathEntity blobItemPath) => await _exists.ExistsAsync(blobItemPath).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task DeleteAsync(BlobPathEntity blobItemPath) => await _delete.DeleteAsync(blobItemPath).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<BlobOpResponse<BlobContentInfo>> WriteAsync(IBlobBinaryWriteDomain blobWriteEntity) => await _write.WriteAsync(blobWriteEntity).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<IList<IBlobListingItem>> BlobListAsync() => await _list.BlobListAsync().ConfigureAwait(false);
}
