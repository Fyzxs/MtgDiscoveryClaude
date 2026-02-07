using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Commands;
using Lib.Adapter.Collections.Queries;
using Lib.Adapter.Collections.Queries.Mappers;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Apis;

/// <summary>
/// Composite service that coordinates all collection adapter operations.
/// Implements the passthrough pattern, delegating to specialized query and command adapters.
///
/// Architecture: AdapterService → Single-Operation Adapters
///
/// This service provides a unified API for all collection operations while internally
/// organizing the implementation across specialized adapters for queries and commands.
/// </summary>
public sealed class CollectionsAdapterService : ICollectionsAdapterService
{
    private readonly ICollectionCommandAdapter _commandAdapter;
    private readonly IDefaultCollectionAdapter _defaultCollectionAdapter;
    private readonly ICollectionsByOwnerAdapter _collectionsByOwnerAdapter;
    private readonly ICollectionByIdAdapter _collectionByIdAdapter;
    private readonly IAccessibleCollectionsAdapter _accessibleCollectionsAdapter;
    private readonly IOwnerIdXfrToUserIdXfrMapper _ownerIdXfrToUserIdXfrMapper;

    public CollectionsAdapterService(ILogger logger) : this(
        new CollectionCommandAdapter(logger),
        new DefaultCollectionAdapter(logger),
        new CollectionsByOwnerAdapter(logger),
        new CollectionByIdAdapter(logger),
        new AccessibleCollectionsAdapter(logger),
        new OwnerIdXfrToUserIdXfrMapper())
    { }

    private CollectionsAdapterService(
        ICollectionCommandAdapter commandAdapter,
        IDefaultCollectionAdapter defaultCollectionAdapter,
        ICollectionsByOwnerAdapter collectionsByOwnerAdapter,
        ICollectionByIdAdapter collectionByIdAdapter,
        IAccessibleCollectionsAdapter accessibleCollectionsAdapter,
        IOwnerIdXfrToUserIdXfrMapper ownerIdXfrToUserIdXfrMapper)
    {
        _commandAdapter = commandAdapter;
        _defaultCollectionAdapter = defaultCollectionAdapter;
        _collectionsByOwnerAdapter = collectionsByOwnerAdapter;
        _collectionByIdAdapter = collectionByIdAdapter;
        _accessibleCollectionsAdapter = accessibleCollectionsAdapter;
        _ownerIdXfrToUserIdXfrMapper = ownerIdXfrToUserIdXfrMapper;
    }

    public async Task<IOperationResponse<CollectionExtEntity>> CreateCollectionAsync(ICollectionXfrEntity entity, CancellationToken cancellationToken)
        => await _commandAdapter.CreateCollectionAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> RenameCollectionAsync(IRenameCollectionXfrEntity entity, CancellationToken cancellationToken)
        => await _commandAdapter.RenameCollectionAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityXfrEntity entity, CancellationToken cancellationToken)
        => await _commandAdapter.UpdateCollectionVisibilityAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessXfrEntity entity, CancellationToken cancellationToken)
        => await _commandAdapter.GrantCollectionAccessAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessXfrEntity entity, CancellationToken cancellationToken)
        => await _commandAdapter.RevokeCollectionAccessAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> DeleteCollectionAsync(IDeleteCollectionXfrEntity entity, CancellationToken cancellationToken)
        => await _commandAdapter.DeleteCollectionAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipXfrEntity entity, CancellationToken cancellationToken)
        => await _commandAdapter.TransferCollectionOwnershipAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionExtEntity>> GetDefaultCollectionAsync(IOwnerIdXfrEntity args, CancellationToken cancellationToken)
    {
        IUserIdXfrEntity userIdArgs = await _ownerIdXfrToUserIdXfrMapper.Map(args).ConfigureAwait(false);
        return await _defaultCollectionAdapter.Execute(userIdArgs, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> GetCollectionsByOwnerAsync(IOwnerIdXfrEntity args, CancellationToken cancellationToken)
    {
        IUserIdXfrEntity userIdArgs = await _ownerIdXfrToUserIdXfrMapper.Map(args).ConfigureAwait(false);
        return await _collectionsByOwnerAdapter.Execute(userIdArgs, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<CollectionExtEntity>> GetCollectionByIdAsync(ICollectionIdXfrEntity args, CancellationToken cancellationToken)
        => await _collectionByIdAdapter.Execute(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> GetSharedCollectionsAsync(IUserIdXfrEntity args, CancellationToken cancellationToken)
        => await _accessibleCollectionsAdapter.Execute(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> GetAccessibleCollectionsAsync(IUserIdXfrEntity args, CancellationToken cancellationToken)
        => await _accessibleCollectionsAdapter.Execute(args, cancellationToken).ConfigureAwait(false);
}
