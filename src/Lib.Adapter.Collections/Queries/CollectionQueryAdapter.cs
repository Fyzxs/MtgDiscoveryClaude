using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Entities;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Queries;

internal sealed class CollectionQueryAdapter : ICollectionQueryAdapter
{
    private readonly CollectionsInquisitor _collectionsInquisitor;
    private readonly CollectionGopher _collectionGopher;

    public CollectionQueryAdapter(ILogger logger) : this(new CollectionsInquisitor(logger), new CollectionGopher(logger)) { }

    private CollectionQueryAdapter(CollectionsInquisitor collectionsInquisitor, CollectionGopher collectionGopher)
    {
        _collectionsInquisitor = collectionsInquisitor;
        _collectionGopher = collectionGopher;
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args)
    {
        QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.owner_id = @ownerId AND c.is_default = true")
            .WithParameter("@ownerId", args.OwnerId);

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _collectionsInquisitor
            .QueryAsync<CollectionExtEntity>(query, new PartitionKey(args.OwnerId), CancellationToken.None)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"Failed to query default collection for owner {args.OwnerId}"));
        }

        CollectionExtEntity defaultCollection = queryResponse.Value?.FirstOrDefault();
        if (defaultCollection is null)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new CollectionAdapterException($"No default collection found for owner {args.OwnerId}"));
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(MapToOuf(defaultCollection));
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args)
    {
        QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.owner_id = @ownerId")
            .WithParameter("@ownerId", args.OwnerId);

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _collectionsInquisitor
            .QueryAsync<CollectionExtEntity>(query, new PartitionKey(args.OwnerId), CancellationToken.None)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(
                new CollectionAdapterException($"Failed to query collections for owner {args.OwnerId}"));
        }

        IEnumerable<ICollectionOufEntity> results = queryResponse.Value?.Select(MapToOuf) ?? [];

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(results);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args)
    {
        ReadPointItem readItem = new()
        {
            Id = new ProvidedCosmosItemId(args.CollectionId),
            Partition = new ProvidedPartitionKeyValue(args.OwnerId)
        };

        OpResponse<CollectionExtEntity> ownerReadResponse = await _collectionGopher
            .ReadAsync<CollectionExtEntity>(readItem)
            .ConfigureAwait(false);

        if (ownerReadResponse.IsSuccessful() && ownerReadResponse.Value is not null)
        {
            return new SuccessOperationResponse<ICollectionOufEntity>(MapToOuf(ownerReadResponse.Value));
        }

        QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.id = @collectionId")
            .WithParameter("@collectionId", args.CollectionId);

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _collectionsInquisitor
            .CrossPartitionQueryAsync<CollectionExtEntity>(query, CancellationToken.None)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful() || queryResponse.Value?.Any() is false)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new NotFoundOperationException($"Collection not found: {args.CollectionId}"));
        }

        CollectionExtEntity collection = queryResponse.Value!.First();

        if (collection.Visibility == "public")
        {
            return new SuccessOperationResponse<ICollectionOufEntity>(MapToOuf(collection));
        }

        return new FailureOperationResponse<ICollectionOufEntity>(
            new ForbiddenOperationException("Access denied to private collection"));
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args)
    {
        QueryDefinition query = new QueryDefinition(
            "SELECT * FROM c WHERE EXISTS (SELECT VALUE au FROM au IN c.authorized_users WHERE au.user_id = @userId)")
            .WithParameter("@userId", args.UserId);

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _collectionsInquisitor
            .CrossPartitionQueryAsync<CollectionExtEntity>(query, CancellationToken.None)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(
                new CollectionAdapterException($"Failed to query shared collections for user {args.UserId}"));
        }

        IEnumerable<ICollectionOufEntity> results = queryResponse.Value?.Select(MapToOuf) ?? [];

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(results);
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args)
    {
        IOperationResponse<IEnumerable<ICollectionOufEntity>> ownedResponse = await GetCollectionsByOwnerAsync(new OwnerIdItrEntity { OwnerId = args.UserId })
            .ConfigureAwait(false);

        if (ownedResponse.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(ownedResponse.OuterException);
        }

        IOperationResponse<IEnumerable<ICollectionOufEntity>> sharedResponse = await GetSharedCollectionsAsync(args)
            .ConfigureAwait(false);

        if (sharedResponse.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(sharedResponse.OuterException);
        }

        List<ICollectionOufEntity> combined = [.. ownedResponse.ResponseData];
        combined.AddRange(sharedResponse.ResponseData);

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(combined);
    }

    private static CollectionOufEntity MapToOuf(CollectionExtEntity ext) => new()
    {
        CollectionId = ext.CollectionId,
        OwnerId = ext.OwnerId,
        Name = ext.Name,
        Type = ext.Type,
        Visibility = ext.Visibility,
        IsDefault = ext.IsDefault,
        AuthorizedUsers = ext.AuthorizedUsers?.Select(u => (IAuthorizedUserOufEntity)new AuthorizedUserOufEntity
        {
            UserId = u.UserId,
            Role = u.Role,
            GrantedAt = u.GrantedAt,
            GrantedBy = u.GrantedBy
        }).ToList() ?? [],
        CreatedAt = ext.CreatedAt,
        UpdatedAt = ext.UpdatedAt
    };
}
