using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Commands;

internal sealed class CollectionCommandAdapter : ICollectionCommandAdapter
{
    private readonly ICreateCollectionAdapter _createAdapter;
    private readonly IRenameCollectionAdapter _renameAdapter;
    private readonly IUpdateCollectionVisibilityAdapter _updateVisibilityAdapter;
    private readonly IGrantCollectionAccessAdapter _grantAccessAdapter;
    private readonly IRevokeCollectionAccessAdapter _revokeAccessAdapter;
    private readonly IDeleteCollectionAdapter _deleteAdapter;
    private readonly ITransferCollectionOwnershipAdapter _transferOwnershipAdapter;

    public CollectionCommandAdapter(ILogger logger) : this(
        new CreateCollectionAdapter(logger),
        new RenameCollectionAdapter(logger),
        new UpdateCollectionVisibilityAdapter(logger),
        new GrantCollectionAccessAdapter(logger),
        new RevokeCollectionAccessAdapter(logger),
        new DeleteCollectionAdapter(logger),
        new TransferCollectionOwnershipAdapter(logger))
    { }

    private CollectionCommandAdapter(
        ICreateCollectionAdapter createAdapter,
        IRenameCollectionAdapter renameAdapter,
        IUpdateCollectionVisibilityAdapter updateVisibilityAdapter,
        IGrantCollectionAccessAdapter grantAccessAdapter,
        IRevokeCollectionAccessAdapter revokeAccessAdapter,
        IDeleteCollectionAdapter deleteAdapter,
        ITransferCollectionOwnershipAdapter transferOwnershipAdapter)
    {
        _createAdapter = createAdapter;
        _renameAdapter = renameAdapter;
        _updateVisibilityAdapter = updateVisibilityAdapter;
        _grantAccessAdapter = grantAccessAdapter;
        _revokeAccessAdapter = revokeAccessAdapter;
        _deleteAdapter = deleteAdapter;
        _transferOwnershipAdapter = transferOwnershipAdapter;
    }

    public async Task<IOperationResponse<CollectionExtEntity>> CreateCollectionAsync(
        ICollectionXfrEntity entity, CancellationToken cancellationToken)
        => await _createAdapter.Execute(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> RenameCollectionAsync(
        IRenameCollectionXfrEntity entity, CancellationToken cancellationToken)
        => await _renameAdapter.Execute(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> UpdateCollectionVisibilityAsync(
        IUpdateCollectionVisibilityXfrEntity entity, CancellationToken cancellationToken)
        => await _updateVisibilityAdapter.Execute(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> GrantCollectionAccessAsync(
        IGrantCollectionAccessXfrEntity entity, CancellationToken cancellationToken)
        => await _grantAccessAdapter.Execute(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> RevokeCollectionAccessAsync(
        IRevokeCollectionAccessXfrEntity entity, CancellationToken cancellationToken)
        => await _revokeAccessAdapter.Execute(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> DeleteCollectionAsync(
        IDeleteCollectionXfrEntity entity, CancellationToken cancellationToken)
        => await _deleteAdapter.Execute(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> TransferCollectionOwnershipAsync(
        ITransferCollectionOwnershipXfrEntity entity, CancellationToken cancellationToken)
        => await _transferOwnershipAdapter.Execute(entity, cancellationToken).ConfigureAwait(false);
}
