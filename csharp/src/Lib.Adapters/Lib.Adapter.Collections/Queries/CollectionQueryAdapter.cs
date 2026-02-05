using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Collections.Queries.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Queries;

internal sealed class CollectionQueryAdapter : ICollectionQueryAdapter
{
    private readonly ICosmosInquisitor _collectionsInquisitor;
    private readonly ICosmosGopher _collectionGopher;

    public CollectionQueryAdapter(ILogger logger) : this(
        new CollectionsInquisitor(logger),
        new CollectionGopher(logger))
    { }

    private CollectionQueryAdapter(
        ICosmosInquisitor collectionsInquisitor,
        ICosmosGopher collectionGopher)
    {
        _collectionsInquisitor = collectionsInquisitor;
        _collectionGopher = collectionGopher;
    }

    public async Task<IOperationResponse<CollectionExtEntity>> GetDefaultCollectionAsync(IOwnerIdXfrEntity args, CancellationToken cancellationToken)
    {
        QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.owner_id = @ownerId AND c.is_default = true")
            .WithParameter("@ownerId", args.OwnerId);

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _collectionsInquisitor
            .QueryAsync<CollectionExtEntity>(query, new PartitionKey(args.OwnerId), cancellationToken)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"Failed to query default collection for owner {args.OwnerId}"));
        }

        CollectionExtEntity defaultCollection = queryResponse.Value?.FirstOrDefault();
        if (defaultCollection is null)
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"No default collection found for owner {args.OwnerId}"));
        }

        return new SuccessOperationResponse<CollectionExtEntity>(defaultCollection);
    }

    public async Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> GetCollectionsByOwnerAsync(IOwnerIdXfrEntity args, CancellationToken cancellationToken)
    {
        QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.owner_id = @ownerId")
            .WithParameter("@ownerId", args.OwnerId);

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _collectionsInquisitor
            .QueryAsync<CollectionExtEntity>(query, new PartitionKey(args.OwnerId), cancellationToken)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<CollectionExtEntity>>(
                new CollectionAdapterException($"Failed to query collections for owner {args.OwnerId}"));
        }

        return new SuccessOperationResponse<IEnumerable<CollectionExtEntity>>(queryResponse.Value ?? []);
    }

    public async Task<IOperationResponse<CollectionExtEntity>> GetCollectionByIdAsync(ICollectionIdXfrEntity args, CancellationToken cancellationToken)
    {
        ReadPointItem readItem = new()
        {
            Id = new ProvidedCosmosItemId(args.CollectionId),
            Partition = new ProvidedPartitionKeyValue(args.OwnerId)
        };

        OpResponse<CollectionExtEntity> ownerReadResponse = await _collectionGopher
            .ReadAsync<CollectionExtEntity>(readItem, cancellationToken)
            .ConfigureAwait(false);

        if (ownerReadResponse.IsSuccessful() && ownerReadResponse.Value is not null)
        {
            return new SuccessOperationResponse<CollectionExtEntity>(ownerReadResponse.Value);
        }

        QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.id = @collectionId")
            .WithParameter("@collectionId", args.CollectionId);

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _collectionsInquisitor
            .CrossPartitionQueryAsync<CollectionExtEntity>(query, cancellationToken)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful() || queryResponse.Value?.Any() is false)
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new NotFoundOperationException($"Collection not found: {args.CollectionId}"));
        }

        CollectionExtEntity collection = queryResponse.Value!.First();

        if (collection.Visibility == "public")
        {
            return new SuccessOperationResponse<CollectionExtEntity>(collection);
        }

        return new FailureOperationResponse<CollectionExtEntity>(
            new ForbiddenOperationException("Access denied to private collection"));
    }

    public async Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> GetSharedCollectionsAsync(IUserIdXfrEntity args, CancellationToken cancellationToken)
    {
        QueryDefinition query = new QueryDefinition(
            "SELECT * FROM c WHERE EXISTS (SELECT VALUE au FROM au IN c.authorized_users WHERE au.user_id = @userId)")
            .WithParameter("@userId", args.UserId);

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _collectionsInquisitor
            .CrossPartitionQueryAsync<CollectionExtEntity>(query, cancellationToken)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<CollectionExtEntity>>(
                new CollectionAdapterException($"Failed to query shared collections for user {args.UserId}"));
        }

        return new SuccessOperationResponse<IEnumerable<CollectionExtEntity>>(queryResponse.Value ?? []);
    }

    public async Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> GetAccessibleCollectionsAsync(IUserIdXfrEntity args, CancellationToken cancellationToken)
    {
        IOperationResponse<IEnumerable<CollectionExtEntity>> ownedResponse = await GetCollectionsByOwnerAsync(
            new OwnerIdXfrEntity { OwnerId = args.UserId }, cancellationToken)
            .ConfigureAwait(false);

        if (ownedResponse.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<CollectionExtEntity>>(ownedResponse.OuterException);
        }

        IOperationResponse<IEnumerable<CollectionExtEntity>> sharedResponse = await GetSharedCollectionsAsync(args, cancellationToken)
            .ConfigureAwait(false);

        if (sharedResponse.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<CollectionExtEntity>>(sharedResponse.OuterException);
        }

        List<CollectionExtEntity> combined = [.. ownedResponse.ResponseData];
        combined.AddRange(sharedResponse.ResponseData);

        return new SuccessOperationResponse<IEnumerable<CollectionExtEntity>>(combined);
    }
}
