using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.Collections;

internal sealed class CollectionMigrationOrchestrator : ICollectionMigrationOrchestrator
{
    private readonly ILogger _logger;
    private readonly UserInfoInquisitor _userInfoInquisitor;
    private readonly CollectionsInquisitor _collectionsInquisitor;
    private readonly CollectionScribe _collectionScribe;
    private readonly UserCardsInquisitor _userCardsInquisitor;
    private readonly UserCardsScribe _userCardsScribe;
    private readonly UserWishlistCardsInquisitor _userWishlistCardsInquisitor;
    private readonly UserWishlistCardsScribe _userWishlistCardsScribe;
    private readonly UserSetCardsInquisitor _userSetCardsInquisitor;
    private readonly UserSetCardsScribe _userSetCardsScribe;

    public CollectionMigrationOrchestrator(ILogger logger)
        : this(
            logger,
            new UserInfoInquisitor(logger),
            new CollectionsInquisitor(logger),
            new CollectionScribe(logger),
            new UserCardsInquisitor(logger),
            new UserCardsScribe(logger),
            new UserWishlistCardsInquisitor(logger),
            new UserWishlistCardsScribe(logger),
            new UserSetCardsInquisitor(logger),
            new UserSetCardsScribe(logger))
    { }

    private CollectionMigrationOrchestrator(
        ILogger logger,
        UserInfoInquisitor userInfoInquisitor,
        CollectionsInquisitor collectionsInquisitor,
        CollectionScribe collectionScribe,
        UserCardsInquisitor userCardsInquisitor,
        UserCardsScribe userCardsScribe,
        UserWishlistCardsInquisitor userWishlistCardsInquisitor,
        UserWishlistCardsScribe userWishlistCardsScribe,
        UserSetCardsInquisitor userSetCardsInquisitor,
        UserSetCardsScribe userSetCardsScribe)
    {
        _logger = logger;
        _userInfoInquisitor = userInfoInquisitor;
        _collectionsInquisitor = collectionsInquisitor;
        _collectionScribe = collectionScribe;
        _userCardsInquisitor = userCardsInquisitor;
        _userCardsScribe = userCardsScribe;
        _userWishlistCardsInquisitor = userWishlistCardsInquisitor;
        _userWishlistCardsScribe = userWishlistCardsScribe;
        _userSetCardsInquisitor = userSetCardsInquisitor;
        _userSetCardsScribe = userSetCardsScribe;
    }

    public async Task<CollectionMigrationResult> ExecuteMigrationAsync()
    {
        _logger.LogInformation("Starting collection migration...");

        IEnumerable<UserInfoExtEntity> users = await GetAllUsersAsync().ConfigureAwait(false);
        List<UserInfoExtEntity> userList = [.. users];

        _logger.LogInformation("Found {UserCount} users to process", userList.Count);

        int collectionsCreated = 0;
        int collectionsSkipped = 0;
        int userCardsUpdated = 0;
        int userWishlistCardsUpdated = 0;
        int userSetCardsUpdated = 0;
        int errors = 0;

        foreach (UserInfoExtEntity user in userList)
        {
            try
            {
                UserMigrationResult userResult = await MigrateUserAsync(user).ConfigureAwait(false);

                if (userResult.CollectionCreated)
                {
                    collectionsCreated++;
                }
                else
                {
                    collectionsSkipped++;
                }

                userCardsUpdated += userResult.UserCardsUpdated;
                userWishlistCardsUpdated += userResult.UserWishlistCardsUpdated;
                userSetCardsUpdated += userResult.UserSetCardsUpdated;
            }
            catch (Exception ex) when (ex is not OutOfMemoryException && ex is not StackOverflowException)
            {
                errors++;
                _logger.LogError(ex, "Error migrating user {UserId}", user.UserId);
            }
        }

        _logger.LogInformation("Collection migration completed: {Created} collections created, {Skipped} skipped",
            collectionsCreated, collectionsSkipped);

        return new CollectionMigrationResult
        {
            TotalUsers = userList.Count,
            CollectionsCreated = collectionsCreated,
            CollectionsSkipped = collectionsSkipped,
            UserCardsUpdated = userCardsUpdated,
            UserWishlistCardsUpdated = userWishlistCardsUpdated,
            UserSetCardsUpdated = userSetCardsUpdated,
            Errors = errors
        };
    }

    private async Task<IEnumerable<UserInfoExtEntity>> GetAllUsersAsync()
    {
        QueryDefinition query = new("SELECT * FROM c");

        OpResponse<IEnumerable<UserInfoExtEntity>> response = await _userInfoInquisitor
            .CrossPartitionQueryAsync<UserInfoExtEntity>(query, CancellationToken.None)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            _logger.LogError("Failed to query users: {StatusCode}", response.StatusCode);
            return [];
        }

        return response.Value ?? [];
    }

    private async Task<UserMigrationResult> MigrateUserAsync(UserInfoExtEntity user)
    {
        _logger.LogInformation("Processing user {UserId}...", user.UserId);

        string defaultCollectionId = await EnsureDefaultCollectionAsync(user).ConfigureAwait(false);

        if (string.IsNullOrEmpty(defaultCollectionId))
        {
            return new UserMigrationResult
            {
                CollectionCreated = false,
                UserCardsUpdated = 0,
                UserWishlistCardsUpdated = 0,
                UserSetCardsUpdated = 0
            };
        }

        bool collectionCreated = await CheckIfDefaultCollectionExistsAsync(user.UserId).ConfigureAwait(false) is false;

        int userCardsUpdated = await UpdateUserCardsCollectionIdAsync(user.UserId, defaultCollectionId)
            .ConfigureAwait(false);

        int userWishlistCardsUpdated = await UpdateUserWishlistCardsCollectionIdAsync(user.UserId, defaultCollectionId)
            .ConfigureAwait(false);

        int userSetCardsUpdated = await UpdateUserSetCardsCollectionIdAsync(user.UserId, defaultCollectionId)
            .ConfigureAwait(false);

        return new UserMigrationResult
        {
            CollectionCreated = collectionCreated,
            UserCardsUpdated = userCardsUpdated,
            UserWishlistCardsUpdated = userWishlistCardsUpdated,
            UserSetCardsUpdated = userSetCardsUpdated
        };
    }

    private async Task<string> EnsureDefaultCollectionAsync(UserInfoExtEntity user)
    {
        CollectionExtEntity existingDefault = await GetDefaultCollectionAsync(user.UserId).ConfigureAwait(false);

        if (existingDefault is not null)
        {
            _logger.LogInformation("Default collection already exists for user {UserId}", user.UserId);
            return existingDefault.CollectionId;
        }

        string collectionId = Guid.NewGuid().ToString();
        string now = DateTime.UtcNow.ToString("O");

        CollectionExtEntity newCollection = new()
        {
            CollectionId = collectionId,
            OwnerId = user.UserId,
            Name = "My Collection",
            Type = "default",
            Visibility = "private",
            IsDefault = true,
            AuthorizedUsers = [],
            CreatedAt = now,
            UpdatedAt = now
        };

        OpResponse<CollectionExtEntity> response = await _collectionScribe
            .UpsertAsync(newCollection)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            _logger.LogError("Failed to create default collection for user {UserId}: {StatusCode}",
                user.UserId, response.StatusCode);
            return string.Empty;
        }

        _logger.LogInformation("Created default collection {CollectionId} for user {UserId}",
            collectionId, user.UserId);

        return collectionId;
    }

    private async Task<bool> CheckIfDefaultCollectionExistsAsync(string userId)
    {
        CollectionExtEntity existing = await GetDefaultCollectionAsync(userId).ConfigureAwait(false);
        return existing is not null;
    }

    private async Task<CollectionExtEntity> GetDefaultCollectionAsync(string userId)
    {
        QueryDefinition query = new QueryDefinition(
            "SELECT * FROM c WHERE c.owner_id = @ownerId AND c.is_default = true")
            .WithParameter("@ownerId", userId);

        OpResponse<IEnumerable<CollectionExtEntity>> response = await _collectionsInquisitor
            .QueryAsync<CollectionExtEntity>(query, new PartitionKey(userId), CancellationToken.None)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return null;
        }

        return response.Value?.FirstOrDefault();
    }

    private async Task<int> UpdateUserCardsCollectionIdAsync(string userId, string collectionId)
    {
        QueryDefinition query = new QueryDefinition(
            "SELECT * FROM c WHERE c.user_id = @userId AND (c.collection_id = '' OR NOT IS_DEFINED(c.collection_id))")
            .WithParameter("@userId", userId);

        OpResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsInquisitor
            .QueryAsync<UserCardExtEntity>(query, new PartitionKey(userId), CancellationToken.None)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful() || response.Value is null)
        {
            return 0;
        }

        List<UserCardExtEntity> cardsToUpdate = [.. response.Value];
        int updatedCount = 0;

        foreach (UserCardExtEntity card in cardsToUpdate)
        {
            UserCardExtEntity updatedCard = new()
            {
                UserId = card.UserId,
                CardId = card.CardId,
                SetId = card.SetId,
                CardName = card.CardName,
                SetName = card.SetName,
                SetCode = card.SetCode,
                ReleasedAt = card.ReleasedAt,
                Artist = card.Artist,
                ArtistIds = card.ArtistIds,
                CardNameGuid = card.CardNameGuid,
                CollectedList = card.CollectedList,
                CollectionId = collectionId
            };

            OpResponse<UserCardExtEntity> upsertResponse = await _userCardsScribe
                .UpsertAsync(updatedCard)
                .ConfigureAwait(false);

            if (upsertResponse.IsSuccessful())
            {
                updatedCount++;
            }
        }

        if (updatedCount > 0)
        {
            _logger.LogInformation("Updated {Count} UserCards with collection_id for user {UserId}",
                updatedCount, userId);
        }

        return updatedCount;
    }

    private async Task<int> UpdateUserWishlistCardsCollectionIdAsync(string userId, string collectionId)
    {
        QueryDefinition query = new QueryDefinition(
            "SELECT * FROM c WHERE c.user_id = @userId AND (c.collection_id = '' OR NOT IS_DEFINED(c.collection_id))")
            .WithParameter("@userId", userId);

        OpResponse<IEnumerable<UserWishlistCardExtEntity>> response = await _userWishlistCardsInquisitor
            .QueryAsync<UserWishlistCardExtEntity>(query, new PartitionKey(userId), CancellationToken.None)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful() || response.Value is null)
        {
            return 0;
        }

        List<UserWishlistCardExtEntity> cardsToUpdate = [.. response.Value];
        int updatedCount = 0;

        foreach (UserWishlistCardExtEntity card in cardsToUpdate)
        {
            UserWishlistCardExtEntity updatedCard = new()
            {
                UserId = card.UserId,
                CardId = card.CardId,
                SetId = card.SetId,
                CardName = card.CardName,
                SetName = card.SetName,
                SetCode = card.SetCode,
                ArtistIds = card.ArtistIds,
                CardNameGuid = card.CardNameGuid,
                WishlistItems = card.WishlistItems,
                CreatedAt = card.CreatedAt,
                UpdatedAt = card.UpdatedAt,
                CollectionId = collectionId
            };

            OpResponse<UserWishlistCardExtEntity> upsertResponse = await _userWishlistCardsScribe
                .UpsertAsync(updatedCard)
                .ConfigureAwait(false);

            if (upsertResponse.IsSuccessful())
            {
                updatedCount++;
            }
        }

        if (updatedCount > 0)
        {
            _logger.LogInformation("Updated {Count} UserWishlistCards with collection_id for user {UserId}",
                updatedCount, userId);
        }

        return updatedCount;
    }

    private async Task<int> UpdateUserSetCardsCollectionIdAsync(string userId, string collectionId)
    {
        QueryDefinition query = new QueryDefinition(
            "SELECT * FROM c WHERE c.user_id = @userId AND (c.collection_id = '' OR NOT IS_DEFINED(c.collection_id))")
            .WithParameter("@userId", userId);

        OpResponse<IEnumerable<UserSetCardExtEntity>> response = await _userSetCardsInquisitor
            .QueryAsync<UserSetCardExtEntity>(query, new PartitionKey(userId), CancellationToken.None)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful() || response.Value is null)
        {
            return 0;
        }

        List<UserSetCardExtEntity> setCardsToUpdate = [.. response.Value];
        int updatedCount = 0;

        foreach (UserSetCardExtEntity setCard in setCardsToUpdate)
        {
            UserSetCardExtEntity updatedSetCard = new()
            {
                UserId = setCard.UserId,
                SetId = setCard.SetId,
                TotalCards = setCard.TotalCards,
                UniqueCards = setCard.UniqueCards,
                Collecting = setCard.Collecting,
                Groups = setCard.Groups,
                CollectionId = collectionId
            };

            OpResponse<UserSetCardExtEntity> upsertResponse = await _userSetCardsScribe
                .UpsertAsync(updatedSetCard)
                .ConfigureAwait(false);

            if (upsertResponse.IsSuccessful())
            {
                updatedCount++;
            }
        }

        if (updatedCount > 0)
        {
            _logger.LogInformation("Updated {Count} UserSetCards with collection_id for user {UserId}",
                updatedCount, userId);
        }

        return updatedCount;
    }

    private sealed class UserMigrationResult
    {
        public required bool CollectionCreated { get; init; }
        public required int UserCardsUpdated { get; init; }
        public required int UserWishlistCardsUpdated { get; init; }
        public required int UserSetCardsUpdated { get; init; }
    }
}
