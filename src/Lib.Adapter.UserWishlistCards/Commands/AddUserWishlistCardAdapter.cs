using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Adapter.UserWishlistCards.Commands.Mappers;
using Lib.Adapter.UserWishlistCards.Exceptions;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserWishlistCards.Commands;

internal sealed class AddUserWishlistCardAdapter : IAddUserWishlistCardAdapter
{
    private readonly ICosmosGopher _userWishlistCardsGopher;
    private readonly ICosmosScribe _userWishlistCardsScribe;
    private readonly IAddUserWishlistCardXfrToExtMapper _addUserWishlistCardMapper;

    public AddUserWishlistCardAdapter(ILogger logger) : this(new UserWishlistCardsGopher(logger), new UserWishlistCardsScribe(logger), new AddUserWishlistCardXfrToExtMapper()) { }

    private AddUserWishlistCardAdapter(ICosmosGopher userWishlistCardsGopher, ICosmosScribe userWishlistCardsScribe, IAddUserWishlistCardXfrToExtMapper addUserWishlistCardMapper)
    {
        _userWishlistCardsGopher = userWishlistCardsGopher;
        _userWishlistCardsScribe = userWishlistCardsScribe;
        _addUserWishlistCardMapper = addUserWishlistCardMapper;
    }

    public async Task<IOperationResponse<UserWishlistCardExtEntity>> Execute([NotNull] IAddUserWishlistCardXfrEntity input)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(input.CardId),
            Partition = new ProvidedPartitionKeyValue(input.UserId)
        };
        OpResponse<UserWishlistCardExtEntity> existingResponse = await _userWishlistCardsGopher.ReadAsync<UserWishlistCardExtEntity>(readPoint).ConfigureAwait(false);

        UserWishlistCardExtEntity itemToUpsert;

        if (existingResponse.IsSuccessful())
        {
            UserWishlistCardExtEntity existingItem = existingResponse.Value;
            itemToUpsert = MergeWishlistItems(existingItem, input);
        }
        else
        {
            itemToUpsert = await _addUserWishlistCardMapper.Map(input).ConfigureAwait(false);
        }

        OpResponse<UserWishlistCardExtEntity> upsertResponse = await _userWishlistCardsScribe.UpsertAsync(itemToUpsert).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserWishlistCardExtEntity>(
                new UserWishlistCardsAdapterException($"Failed to add user wishlist card: {upsertResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<UserWishlistCardExtEntity>(upsertResponse.Value);
    }

    private UserWishlistCardExtEntity MergeWishlistItems(UserWishlistCardExtEntity existing, IAddUserWishlistCardXfrEntity newData)
    {
        Dictionary<(string finish, string special), UserWishlistCardDetailsExtEntity> mergedItems = existing.WishlistItems
            .ToDictionary(item => (item.Finish, item.Special));

        IUserWishlistCardDetailsXfrEntity newItem = newData.Details;
        (string finish, string special) key = (newItem.Finish, newItem.Special);

        if (mergedItems.TryGetValue(key, out UserWishlistCardDetailsExtEntity existingItem))
        {
            int newCount = existingItem.Count + newItem.Count;
            if (0 < newCount)
            {
                mergedItems[key] = new UserWishlistCardDetailsExtEntity
                {
                    Finish = existingItem.Finish,
                    Special = existingItem.Special,
                    Count = newCount
                };
            }
            else
            {
                mergedItems.Remove(key);
            }
        }
        else
        {
            if (0 < newItem.Count)
            {
                mergedItems[key] = new UserWishlistCardDetailsExtEntity
                {
                    Finish = newItem.Finish,
                    Special = newItem.Special,
                    Count = newItem.Count
                };
            }
        }

        return new UserWishlistCardExtEntity
        {
            UserId = existing.UserId,
            CardId = existing.CardId,
            SetId = existing.SetId,
            CardName = newData.CardName,
            SetName = newData.SetName,
            SetCode = newData.SetCode,
            ArtistIds = newData.ArtistIds,
            CardNameGuid = newData.CardNameGuid,
            WishlistItems = [.. mergedItems.Values],
            CreatedAt = existing.CreatedAt,
            UpdatedAt = DateTime.UtcNow.ToString("o")
        };
    }
}
