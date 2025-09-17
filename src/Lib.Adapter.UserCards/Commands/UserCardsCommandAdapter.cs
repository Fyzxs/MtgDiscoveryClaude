// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Commands.Mappers;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Commands;

internal sealed class UserCardsCommandAdapter : IUserCardsCommandAdapter
{
    private readonly ICosmosGopher _userCardsGopher;
    private readonly ICosmosScribe _userCardsScribe;
    private readonly IUserCardItrToExtapper _userCardItemMapper;
    private readonly IUserCardExtToItrMapper _userCardCollectionMapper;

    public UserCardsCommandAdapter(ILogger logger) : this(
        new UserCardsGopher(logger),
        new UserCardsScribe(logger),
        new UserCardItrToExtapper(),
        new UserCardExtToItrMapper())
    { }

    internal UserCardsCommandAdapter(
        ICosmosGopher userCardsGopher,
        ICosmosScribe userCardsScribe,
        IUserCardItrToExtapper userCardItemMapper,
        IUserCardExtToItrMapper userCardCollectionMapper)
    {
        _userCardsGopher = userCardsGopher;
        _userCardsScribe = userCardsScribe;
        _userCardItemMapper = userCardItemMapper;
        _userCardCollectionMapper = userCardCollectionMapper;
    }

    public async Task<IOperationResponse<IUserCardItrEntity>> AddUserCardAsync(IUserCardItrEntity userCard)
    {
        // Step 1: Try to read existing record
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(userCard.CardId),
            Partition = new ProvidedPartitionKeyValue(userCard.UserId)
        };
        OpResponse<UserCardExtEntity> existingResponse = await _userCardsGopher.ReadAsync<UserCardExtEntity>(readPoint).ConfigureAwait(false);

        UserCardExtEntity itemToUpsert;

        if (existingResponse.IsSuccessful())
        {
            // Step 2: Merge with existing collected items
            UserCardExtEntity existingItem = existingResponse.Value;
            itemToUpsert = MergeCollectedItems(existingItem, userCard);
        }
        else
        {
            // Step 3: Create new item if none exists
            itemToUpsert = await _userCardItemMapper.Map(userCard).ConfigureAwait(false);
        }

        // Step 4: Upsert the merged/new item
        OpResponse<UserCardExtEntity> upsertResponse = await _userCardsScribe.UpsertAsync(itemToUpsert).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IUserCardItrEntity>(
                new UserCardsAdapterException($"Failed to add user card: {upsertResponse.StatusCode}"));
        }

        // Step 5: Map and return the result
        IUserCardItrEntity resultEntity = await _userCardCollectionMapper.Map(upsertResponse.Value).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserCardItrEntity>(resultEntity);
    }

    private UserCardExtEntity MergeCollectedItems(UserCardExtEntity existing, IUserCardItrEntity newData)
    {
        // Create a dictionary for efficient merging based on finish + special combination
        Dictionary<(string finish, string special), UserCardDetailsExtEntity> mergedItems = existing.CollectedList
            .ToDictionary(item => (item.Finish, item.Special));

        // Merge or add new collected items
        foreach (IUserCardDetailsItrEntity newItem in newData.CollectedList)
        {
            (string finish, string special) key = (newItem.Finish, newItem.Special);

            if (mergedItems.TryGetValue(key, out UserCardDetailsExtEntity existingItem))
            {
                // Update count for existing finish/special combination
                mergedItems[key] = new UserCardDetailsExtEntity
                {
                    Finish = existingItem.Finish,
                    Special = existingItem.Special,
                    Count = existingItem.Count + newItem.Count
                };
            }
            else
            {
                // Add new finish/special combination
                mergedItems[key] = new UserCardDetailsExtEntity
                {
                    Finish = newItem.Finish,
                    Special = newItem.Special,
                    Count = newItem.Count
                };
            }
        }

        // Return updated item
        return new UserCardExtEntity
        {
            UserId = existing.UserId,
            CardId = existing.CardId,
            SetId = existing.SetId,
            CollectedList = [.. mergedItems.Values]
        };
    }
}
