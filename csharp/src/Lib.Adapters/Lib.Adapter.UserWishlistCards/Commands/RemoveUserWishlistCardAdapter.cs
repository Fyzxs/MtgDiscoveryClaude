using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Janitors;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Adapter.UserWishlistCards.Exceptions;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserWishlistCards.Commands;

internal sealed class RemoveUserWishlistCardAdapter : IRemoveUserWishlistCardAdapter
{
    private readonly ICosmosGopher _userWishlistCardsGopher;
    private readonly ICosmosScribe _userWishlistCardsScribe;
    private readonly ICosmosJanitor _userWishlistCardsJanitor;

    public RemoveUserWishlistCardAdapter(ILogger logger) : this(
        new UserWishlistCardsGopher(logger),
        new UserWishlistCardsScribe(logger),
        new UserWishlistCardsJanitor(logger))
    { }

    private RemoveUserWishlistCardAdapter(
        ICosmosGopher userWishlistCardsGopher,
        ICosmosScribe userWishlistCardsScribe,
        ICosmosJanitor userWishlistCardsJanitor)
    {
        _userWishlistCardsGopher = userWishlistCardsGopher;
        _userWishlistCardsScribe = userWishlistCardsScribe;
        _userWishlistCardsJanitor = userWishlistCardsJanitor;
    }

    public async Task<IOperationResponse<UserWishlistCardExtEntity>> Execute([NotNull] IRemoveUserWishlistCardXfrEntity input, CancellationToken cancellationToken)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(input.CardId),
            Partition = new ProvidedPartitionKeyValue(input.UserId)
        };
        OpResponse<UserWishlistCardExtEntity> existingResponse = await _userWishlistCardsGopher.ReadAsync<UserWishlistCardExtEntity>(readPoint, cancellationToken).ConfigureAwait(false);

        if (existingResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserWishlistCardExtEntity>(
                new UserWishlistCardsAdapterException($"Wishlist card not found: {input.CardId}"));
        }

        UserWishlistCardExtEntity existingItem = existingResponse.Value;
        UserWishlistCardExtEntity itemToUpsert = RemoveWishlistItem(existingItem, input);

        if (itemToUpsert.WishlistItems.Any() is false)
        {
            DeletePointItem deletePoint = new()
            {
                Id = new ProvidedCosmosItemId(input.CardId),
                Partition = new ProvidedPartitionKeyValue(input.UserId)
            };

            OpResponse<UserWishlistCardExtEntity> deleteResponse = await _userWishlistCardsJanitor.DeleteAsync<UserWishlistCardExtEntity>(deletePoint, cancellationToken).ConfigureAwait(false);

            if (deleteResponse.IsNotSuccessful())
            {
                return new FailureOperationResponse<UserWishlistCardExtEntity>(
                    new UserWishlistCardsAdapterException($"Failed to delete wishlist card: {deleteResponse.StatusCode}"));
            }

            return new SuccessOperationResponse<UserWishlistCardExtEntity>(existingItem);
        }

        OpResponse<UserWishlistCardExtEntity> upsertResponse = await _userWishlistCardsScribe.UpsertAsync(itemToUpsert, cancellationToken).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserWishlistCardExtEntity>(
                new UserWishlistCardsAdapterException($"Failed to remove user wishlist card: {upsertResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<UserWishlistCardExtEntity>(upsertResponse.Value);
    }

    private UserWishlistCardExtEntity RemoveWishlistItem(UserWishlistCardExtEntity existing, IRemoveUserWishlistCardXfrEntity removeData)
    {
        Dictionary<(string finish, string special), UserWishlistCardDetailsExtEntity> items = existing.WishlistItems
            .ToDictionary(item => (item.Finish, item.Special));

        IUserWishlistCardDetailsXfrEntity removeItem = removeData.Details;
        (string finish, string special) key = (removeItem.Finish, removeItem.Special);

        if (items.TryGetValue(key, out UserWishlistCardDetailsExtEntity existingItem))
        {
            int newCount = existingItem.Count - removeItem.Count;
            if (0 < newCount)
            {
                items[key] = new UserWishlistCardDetailsExtEntity
                {
                    Finish = existingItem.Finish,
                    Special = existingItem.Special,
                    Count = newCount
                };
            }
            else
            {
                items.Remove(key);
            }
        }

        return new UserWishlistCardExtEntity
        {
            UserId = existing.UserId,
            CardId = existing.CardId,
            SetId = existing.SetId,
            CardName = existing.CardName,
            SetName = existing.SetName,
            SetCode = existing.SetCode,
            ArtistIds = existing.ArtistIds,
            CardNameGuid = existing.CardNameGuid,
            WishlistItems = [.. items.Values],
            CreatedAt = existing.CreatedAt,
            UpdatedAt = DateTime.UtcNow.ToString("o")
        };
    }
}
