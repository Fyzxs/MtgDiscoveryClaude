using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Entities;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Collections.Queries.Mappers;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
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
    private readonly ICosmosInquisitor _collectionsInquisitor;
    private readonly ICosmosGopher _collectionGopher;
    private readonly ICollectionExtToOufMapper _mapper;

    public CollectionQueryAdapter(ILogger logger) : this(
        new CollectionsInquisitor(logger),
        new CollectionGopher(logger),
        new CollectionExtToOufMapper())
    { }

    private CollectionQueryAdapter(
        ICosmosInquisitor collectionsInquisitor,
        ICosmosGopher collectionGopher,
        ICollectionExtToOufMapper mapper)
    {
        _collectionsInquisitor = collectionsInquisitor;
        _collectionGopher = collectionGopher;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken)
    {
        QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.owner_id = @ownerId AND c.is_default = true")
            .WithParameter("@ownerId", args.OwnerId);

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _collectionsInquisitor
            .QueryAsync<CollectionExtEntity>(query, new PartitionKey(args.OwnerId), cancellationToken)
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

        ICollectionOufEntity oufEntity = await _mapper.Map(defaultCollection).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken)
    {
        QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.owner_id = @ownerId")
            .WithParameter("@ownerId", args.OwnerId);

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _collectionsInquisitor
            .QueryAsync<CollectionExtEntity>(query, new PartitionKey(args.OwnerId), cancellationToken)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(
                new CollectionAdapterException($"Failed to query collections for owner {args.OwnerId}"));
        }

        ICollectionOufEntity[] results = await Task.WhenAll(
            queryResponse.Value?.Select(ext => _mapper.Map(ext)) ?? []).ConfigureAwait(false);

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(results);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args, CancellationToken cancellationToken)
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
            ICollectionOufEntity oufEntity = await _mapper.Map(ownerReadResponse.Value).ConfigureAwait(false);
            return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
        }

        QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.id = @collectionId")
            .WithParameter("@collectionId", args.CollectionId);

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _collectionsInquisitor
            .CrossPartitionQueryAsync<CollectionExtEntity>(query, cancellationToken)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful() || queryResponse.Value?.Any() is false)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(
                new NotFoundOperationException($"Collection not found: {args.CollectionId}"));
        }

        CollectionExtEntity collection = queryResponse.Value!.First();

        if (collection.Visibility == "public")
        {
            ICollectionOufEntity publicOufEntity = await _mapper.Map(collection).ConfigureAwait(false);
            return new SuccessOperationResponse<ICollectionOufEntity>(publicOufEntity);
        }

        return new FailureOperationResponse<ICollectionOufEntity>(
            new ForbiddenOperationException("Access denied to private collection"));
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken)
    {
        QueryDefinition query = new QueryDefinition(
            "SELECT * FROM c WHERE EXISTS (SELECT VALUE au FROM au IN c.authorized_users WHERE au.user_id = @userId)")
            .WithParameter("@userId", args.UserId);

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _collectionsInquisitor
            .CrossPartitionQueryAsync<CollectionExtEntity>(query, cancellationToken)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(
                new CollectionAdapterException($"Failed to query shared collections for user {args.UserId}"));
        }

        ICollectionOufEntity[] results = await Task.WhenAll(
            queryResponse.Value?.Select(ext => _mapper.Map(ext)) ?? []).ConfigureAwait(false);

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(results);
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken)
    {
        IOperationResponse<IEnumerable<ICollectionOufEntity>> ownedResponse = await GetCollectionsByOwnerAsync(new OwnerIdItrEntity { OwnerId = args.UserId }, cancellationToken)
            .ConfigureAwait(false);

        if (ownedResponse.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(ownedResponse.OuterException);
        }

        IOperationResponse<IEnumerable<ICollectionOufEntity>> sharedResponse = await GetSharedCollectionsAsync(args, cancellationToken)
            .ConfigureAwait(false);

        if (sharedResponse.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(sharedResponse.OuterException);
        }

        List<ICollectionOufEntity> combined = [.. ownedResponse.ResponseData];
        combined.AddRange(sharedResponse.ResponseData);

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(combined);
    }
}
