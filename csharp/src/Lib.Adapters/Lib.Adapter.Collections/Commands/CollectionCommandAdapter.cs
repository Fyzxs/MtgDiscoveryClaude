using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Collections.Queries.Mappers;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Janitors;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Commands;

internal sealed class CollectionCommandAdapter : ICollectionCommandAdapter
{
    private readonly ICosmosScribe _collectionScribe;
    private readonly ICosmosGopher _collectionGopher;
    private readonly ICosmosContainerDeleteOperator _collectionJanitor;
    private readonly ICollectionExtToOufMapper _mapper;

    public CollectionCommandAdapter(ILogger logger) : this(
        new CollectionScribe(logger),
        new CollectionGopher(logger),
        new CollectionJanitor(logger),
        new CollectionExtToOufMapper())
    { }

    private CollectionCommandAdapter(
        ICosmosScribe collectionScribe,
        ICosmosGopher collectionGopher,
        ICosmosContainerDeleteOperator collectionJanitor,
        ICollectionExtToOufMapper mapper)
    {
        _collectionScribe = collectionScribe;
        _collectionGopher = collectionGopher;
        _collectionJanitor = collectionJanitor;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity, CancellationToken cancellationToken)
    {
        CollectionExtEntity extEntity = new()
        {
            CollectionId = entity.CollectionId,
            OwnerId = entity.OwnerId,
            Name = entity.Name,
            Type = entity.Type,
            Visibility = entity.Visibility,
            IsDefault = entity.IsDefault,
            AuthorizedUsers = MapAuthorizedUsersToExt(entity.AuthorizedUsers),
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };

        OpResponse<CollectionExtEntity> upsertResponse = await _collectionScribe
            .UpsertAsync(extEntity, cancellationToken)
            .ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Failed to create collection {entity.CollectionId}: {upsertResponse.StatusCode}"));
        }

        ICollectionOufEntity oufEntity = await _mapper.Map(upsertResponse.Value!).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity, CancellationToken cancellationToken)
    {
        ReadPointItem readItem = new()
        {
            Id = new ProvidedCosmosItemId(entity.CollectionId),
            Partition = new ProvidedPartitionKeyValue(entity.OwnerId)
        };

        OpResponse<CollectionExtEntity> existingResponse = await _collectionGopher
            .ReadAsync<CollectionExtEntity>(readItem, cancellationToken)
            .ConfigureAwait(false);

        if (existingResponse.IsNotSuccessful() || existingResponse.Value is null)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Collection not found: {entity.CollectionId}"));
        }

        CollectionExtEntity existing = existingResponse.Value;

        CollectionExtEntity updated = new()
        {
            CollectionId = existing.CollectionId,
            OwnerId = existing.OwnerId,
            Name = entity.Name,
            Type = existing.Type,
            Visibility = existing.Visibility,
            IsDefault = existing.IsDefault,
            AuthorizedUsers = existing.AuthorizedUsers,
            CreatedAt = existing.CreatedAt,
            UpdatedAt = DateTime.UtcNow.ToString("o")
        };

        OpResponse<CollectionExtEntity> upsertResponse = await _collectionScribe
            .UpsertAsync(updated, cancellationToken)
            .ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Failed to rename collection {entity.CollectionId}: {upsertResponse.StatusCode}"));
        }

        ICollectionOufEntity oufEntity = await _mapper.Map(upsertResponse.Value!).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity, CancellationToken cancellationToken)
    {
        ReadPointItem readItem = new()
        {
            Id = new ProvidedCosmosItemId(entity.CollectionId),
            Partition = new ProvidedPartitionKeyValue(entity.OwnerId)
        };

        OpResponse<CollectionExtEntity> existingResponse = await _collectionGopher
            .ReadAsync<CollectionExtEntity>(readItem, cancellationToken)
            .ConfigureAwait(false);

        if (existingResponse.IsNotSuccessful() || existingResponse.Value is null)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Collection not found: {entity.CollectionId}"));
        }

        CollectionExtEntity existing = existingResponse.Value;

        CollectionExtEntity updated = new()
        {
            CollectionId = existing.CollectionId,
            OwnerId = existing.OwnerId,
            Name = existing.Name,
            Type = existing.Type,
            Visibility = entity.Visibility,
            IsDefault = existing.IsDefault,
            AuthorizedUsers = existing.AuthorizedUsers,
            CreatedAt = existing.CreatedAt,
            UpdatedAt = DateTime.UtcNow.ToString("o")
        };

        OpResponse<CollectionExtEntity> upsertResponse = await _collectionScribe
            .UpsertAsync(updated, cancellationToken)
            .ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Failed to update collection visibility {entity.CollectionId}: {upsertResponse.StatusCode}"));
        }

        ICollectionOufEntity oufEntity = await _mapper.Map(upsertResponse.Value!).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity, CancellationToken cancellationToken)
    {
        ReadPointItem readItem = new()
        {
            Id = new ProvidedCosmosItemId(entity.CollectionId),
            Partition = new ProvidedPartitionKeyValue(entity.GrantorUserId)
        };

        OpResponse<CollectionExtEntity> existingResponse = await _collectionGopher
            .ReadAsync<CollectionExtEntity>(readItem, cancellationToken)
            .ConfigureAwait(false);

        if (existingResponse.IsNotSuccessful() || existingResponse.Value is null)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Collection not found: {entity.CollectionId}"));
        }

        CollectionExtEntity existing = existingResponse.Value;

        List<AuthorizedUserExtEntity> authorizedUsers = [.. existing.AuthorizedUsers];
        AuthorizedUserExtEntity existingUser = authorizedUsers.Find(u => u.UserId == entity.TargetUserId);

        if (existingUser is not null)
        {
            authorizedUsers.Remove(existingUser);
        }

        authorizedUsers.Add(new AuthorizedUserExtEntity
        {
            UserId = entity.TargetUserId,
            Role = entity.Role,
            GrantedAt = DateTime.UtcNow.ToString("o"),
            GrantedBy = entity.GrantorUserId
        });

        CollectionExtEntity updated = new()
        {
            CollectionId = existing.CollectionId,
            OwnerId = existing.OwnerId,
            Name = existing.Name,
            Type = existing.Type,
            Visibility = existing.Visibility,
            IsDefault = existing.IsDefault,
            AuthorizedUsers = authorizedUsers,
            CreatedAt = existing.CreatedAt,
            UpdatedAt = DateTime.UtcNow.ToString("o")
        };

        OpResponse<CollectionExtEntity> upsertResponse = await _collectionScribe
            .UpsertAsync(updated, cancellationToken)
            .ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Failed to grant collection access {entity.CollectionId}: {upsertResponse.StatusCode}"));
        }

        ICollectionOufEntity oufEntity = await _mapper.Map(upsertResponse.Value!).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity, CancellationToken cancellationToken)
    {
        ReadPointItem readItem = new()
        {
            Id = new ProvidedCosmosItemId(entity.CollectionId),
            Partition = new ProvidedPartitionKeyValue(entity.RevokerUserId)
        };

        OpResponse<CollectionExtEntity> existingResponse = await _collectionGopher
            .ReadAsync<CollectionExtEntity>(readItem, cancellationToken)
            .ConfigureAwait(false);

        if (existingResponse.IsNotSuccessful() || existingResponse.Value is null)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Collection not found: {entity.CollectionId}"));
        }

        CollectionExtEntity existing = existingResponse.Value;

        List<AuthorizedUserExtEntity> authorizedUsers = [.. existing.AuthorizedUsers];
        int removedCount = authorizedUsers.RemoveAll(u => u.UserId == entity.TargetUserId);

        if (removedCount == 0)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"User {entity.TargetUserId} is not authorized on collection {entity.CollectionId}"));
        }

        CollectionExtEntity updated = new()
        {
            CollectionId = existing.CollectionId,
            OwnerId = existing.OwnerId,
            Name = existing.Name,
            Type = existing.Type,
            Visibility = existing.Visibility,
            IsDefault = existing.IsDefault,
            AuthorizedUsers = authorizedUsers,
            CreatedAt = existing.CreatedAt,
            UpdatedAt = DateTime.UtcNow.ToString("o")
        };

        OpResponse<CollectionExtEntity> upsertResponse = await _collectionScribe
            .UpsertAsync(updated, cancellationToken)
            .ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Failed to revoke collection access {entity.CollectionId}: {upsertResponse.StatusCode}"));
        }

        ICollectionOufEntity oufEntity = await _mapper.Map(upsertResponse.Value!).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity, CancellationToken cancellationToken)
    {
        ReadPointItem readItem = new()
        {
            Id = new ProvidedCosmosItemId(entity.CollectionId),
            Partition = new ProvidedPartitionKeyValue(entity.OwnerId)
        };

        OpResponse<CollectionExtEntity> existingResponse = await _collectionGopher
            .ReadAsync<CollectionExtEntity>(readItem, cancellationToken)
            .ConfigureAwait(false);

        if (existingResponse.IsNotSuccessful() || existingResponse.Value is null)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Collection not found: {entity.CollectionId}"));
        }

        CollectionExtEntity existing = existingResponse.Value;

        if (existing.IsDefault)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException("Cannot delete the default collection"));
        }

        DeletePointItem deleteItem = new()
        {
            Id = new ProvidedCosmosItemId(entity.CollectionId),
            Partition = new ProvidedPartitionKeyValue(entity.OwnerId)
        };

        OpResponse<CollectionExtEntity> deleteResponse = await _collectionJanitor
            .DeleteAsync<CollectionExtEntity>(deleteItem, cancellationToken)
            .ConfigureAwait(false);

        if (deleteResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Failed to delete collection {entity.CollectionId}: {deleteResponse.StatusCode}"));
        }

        ICollectionOufEntity oufEntity = await _mapper.Map(existing).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity, CancellationToken cancellationToken)
    {
        ReadPointItem readItem = new()
        {
            Id = new ProvidedCosmosItemId(entity.CollectionId),
            Partition = new ProvidedPartitionKeyValue(entity.CurrentOwnerId)
        };

        OpResponse<CollectionExtEntity> existingResponse = await _collectionGopher
            .ReadAsync<CollectionExtEntity>(readItem, cancellationToken)
            .ConfigureAwait(false);

        if (existingResponse.IsNotSuccessful() || existingResponse.Value is null)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Collection not found: {entity.CollectionId}"));
        }

        CollectionExtEntity existing = existingResponse.Value;

        if (existing.IsDefault)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException("Cannot transfer ownership of the default collection"));
        }

        List<AuthorizedUserExtEntity> authorizedUsers = [.. existing.AuthorizedUsers];
        authorizedUsers.RemoveAll(u => u.UserId == entity.TargetUserId);

        string nowTimestamp = DateTime.UtcNow.ToString("o");
        authorizedUsers.Add(new AuthorizedUserExtEntity
        {
            UserId = entity.CurrentOwnerId,
            Role = "admin",
            GrantedAt = nowTimestamp,
            GrantedBy = entity.CurrentOwnerId
        });

        DeletePointItem deleteItem = new()
        {
            Id = new ProvidedCosmosItemId(entity.CollectionId),
            Partition = new ProvidedPartitionKeyValue(entity.CurrentOwnerId)
        };

        OpResponse<CollectionExtEntity> deleteResponse = await _collectionJanitor
            .DeleteAsync<CollectionExtEntity>(deleteItem, cancellationToken)
            .ConfigureAwait(false);

        if (deleteResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Failed to remove collection from original owner: {deleteResponse.StatusCode}"));
        }

        CollectionExtEntity updated = new()
        {
            CollectionId = existing.CollectionId,
            OwnerId = entity.TargetUserId,
            Name = existing.Name,
            Type = existing.Type,
            Visibility = existing.Visibility,
            IsDefault = false,
            AuthorizedUsers = authorizedUsers,
            CreatedAt = existing.CreatedAt,
            UpdatedAt = nowTimestamp
        };

        OpResponse<CollectionExtEntity> upsertResponse = await _collectionScribe
            .UpsertAsync(updated, cancellationToken)
            .ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Failed to transfer collection {entity.CollectionId}: {upsertResponse.StatusCode}"));
        }

        ICollectionOufEntity oufEntity = await _mapper.Map(upsertResponse.Value!).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    private static IEnumerable<AuthorizedUserExtEntity> MapAuthorizedUsersToExt(IEnumerable<IAuthorizedUserItrEntity> users)
    {
        foreach (IAuthorizedUserItrEntity user in users)
        {
            yield return new AuthorizedUserExtEntity
            {
                UserId = user.UserId,
                Role = user.Role,
                GrantedAt = user.GrantedAt,
                GrantedBy = user.GrantedBy
            };
        }
    }
}
