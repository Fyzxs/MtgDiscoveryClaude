using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
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
    private readonly IUserCardItemMapper _userCardItemMapper;
    private readonly IUserCardCollectionItrEntityMapper _userCardCollectionMapper;
    private readonly ILogger _logger;

    public UserCardsCommandAdapter(ILogger logger) : this(
        new UserCardsGopher(logger),
        new UserCardsScribe(logger),
        new UserCardItemMapper(),
        new UserCardCollectionItrEntityMapper(),
        logger)
    { }

    internal UserCardsCommandAdapter(
        ICosmosGopher userCardsGopher,
        ICosmosScribe userCardsScribe,
        IUserCardItemMapper userCardItemMapper,
        IUserCardCollectionItrEntityMapper userCardCollectionMapper,
        ILogger logger)
    {
        _userCardsGopher = userCardsGopher;
        _userCardsScribe = userCardsScribe;
        _userCardItemMapper = userCardItemMapper;
        _userCardCollectionMapper = userCardCollectionMapper;
        _logger = logger;
    }

    public async Task<IOperationResponse<IUserCardCollectionItrEntity>> AddUserCardAsync(IUserCardCollectionItrEntity userCard)
    {
        // Step 1: Try to read existing record
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(userCard.CardId),
            Partition = new ProvidedPartitionKeyValue(userCard.UserId)
        };
        OpResponse<UserCardItem> existingResponse = await _userCardsGopher.ReadAsync<UserCardItem>(readPoint).ConfigureAwait(false);

        UserCardItem itemToUpsert;

        if (existingResponse.IsSuccessful())
        {
            // Step 2: Merge with existing collected items
            UserCardItem existingItem = existingResponse.Value;
            itemToUpsert = MergeCollectedItems(existingItem, userCard);
        }
        else
        {
            // Step 3: Create new item if none exists
            itemToUpsert = await _userCardItemMapper.Map(userCard).ConfigureAwait(false);
        }

        // Step 4: Upsert the merged/new item
        OpResponse<UserCardItem> upsertResponse = await _userCardsScribe.UpsertAsync(itemToUpsert).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new UserCardsAdapterException($"Failed to add user card: {upsertResponse.StatusCode}"));
        }

        // Step 5: Map and return the result
        IUserCardCollectionItrEntity resultEntity = await _userCardCollectionMapper.Map(upsertResponse.Value).ConfigureAwait(false);
        return new SuccessOperationResponse<IUserCardCollectionItrEntity>(resultEntity);
    }

    private UserCardItem MergeCollectedItems(UserCardItem existing, IUserCardCollectionItrEntity newData)
    {
        // Create a dictionary for efficient merging based on finish + special combination
        Dictionary<(string finish, string special), CollectedItem> mergedItems = existing.CollectedList
            .ToDictionary(item => (item.Finish, item.Special ?? "none"));

        // Merge or add new collected items
        foreach (ICollectedItemItrEntity newItem in newData.CollectedList)
        {
            (string finish, string special) key = (newItem.Finish, newItem.Special ?? "none");

            if (mergedItems.TryGetValue(key, out CollectedItem existingItem))
            {
                // Update count for existing finish/special combination
                mergedItems[key] = new CollectedItem
                {
                    Finish = existingItem.Finish,
                    Special = existingItem.Special,
                    Count = existingItem.Count + newItem.Count
                };
            }
            else
            {
                // Add new finish/special combination
                mergedItems[key] = new CollectedItem
                {
                    Finish = newItem.Finish,
                    Special = newItem.Special,
                    Count = newItem.Count
                };
            }
        }

        // Return updated item
        return new UserCardItem
        {
            UserId = existing.UserId,
            CardId = existing.CardId,
            SetId = existing.SetId,
            CollectedList = mergedItems.Values.ToList()
        };
    }
}
