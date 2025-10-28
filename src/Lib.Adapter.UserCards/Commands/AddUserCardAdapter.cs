using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Commands.Mappers;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Commands;

/// <summary>
/// Adds or updates a card in a user's collection with intelligent merging logic.
/// </summary>
internal sealed class AddUserCardAdapter : IAddUserCardAdapter
{
    private readonly ICosmosGopher _userCardsGopher;
    private readonly ICosmosScribe _userCardsScribe;
    private readonly IAddUserCardXfrToExtMapper _addUserCardMapper;

    public AddUserCardAdapter(ILogger logger) : this(new UserCardsGopher(logger), new UserCardsScribe(logger), new AddUserCardXfrToExtMapper()) { }

    private AddUserCardAdapter(ICosmosGopher userCardsGopher, ICosmosScribe userCardsScribe, IAddUserCardXfrToExtMapper addUserCardMapper)
    {
        _userCardsGopher = userCardsGopher;
        _userCardsScribe = userCardsScribe;
        _addUserCardMapper = addUserCardMapper;
    }

    public async Task<IOperationResponse<UserCardExtEntity>> Execute([NotNull] IAddUserCardXfrEntity input)
    {
        // Step 1: Try to read existing record
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(input.CardId),
            Partition = new ProvidedPartitionKeyValue(input.UserId)
        };
        OpResponse<UserCardExtEntity> existingResponse = await _userCardsGopher.ReadAsync<UserCardExtEntity>(readPoint).ConfigureAwait(false);

        UserCardExtEntity itemToUpsert;

        if (existingResponse.IsSuccessful())
        {
            // Step 2: Merge with existing collected items
            UserCardExtEntity existingItem = existingResponse.Value;
            itemToUpsert = MergeCollectedItems(existingItem, input);
        }
        else
        {
            // Step 3: Create new item if none exists
            itemToUpsert = await _addUserCardMapper.Map(input).ConfigureAwait(false);
        }

        // Step 4: Upsert the merged/new item
        OpResponse<UserCardExtEntity> upsertResponse = await _userCardsScribe.UpsertAsync(itemToUpsert).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserCardExtEntity>(
                new UserCardsAdapterException($"Failed to add user card: {upsertResponse.StatusCode}"));
        }

        // Step 5: Return the raw ExtEntity result
        return new SuccessOperationResponse<UserCardExtEntity>(upsertResponse.Value);
    }

    private UserCardExtEntity MergeCollectedItems(UserCardExtEntity existing, IAddUserCardXfrEntity newData)
    {
        // Create a dictionary for efficient merging based on finish + special combination
        Dictionary<(string finish, string special), UserCardDetailsExtEntity> mergedItems = existing.CollectedList
            .ToDictionary(item => (item.Finish, item.Special));

        IUserCardDetailsXfrEntity newItem = newData.Details;
        (string finish, string special) key = (newItem.Finish, newItem.Special);

        if (mergedItems.TryGetValue(key, out UserCardDetailsExtEntity existingItem))
        {
            // Update count for existing finish/special combination
            int newCount = existingItem.Count + newItem.Count;
            if (newCount < 0)
            {
                newCount = 0;
            }

            mergedItems[key] = new UserCardDetailsExtEntity
            {
                Finish = existingItem.Finish,
                Special = existingItem.Special,
                Count = newCount
                // setGroupId from newItem intentionally omitted - used for aggregation only
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
                // setGroupId from newItem intentionally omitted - used for aggregation only
            };
        }

        // Return updated item with fresh metadata from newData
        return new UserCardExtEntity
        {
            UserId = existing.UserId,
            CardId = existing.CardId,
            SetId = existing.SetId,
            ArtistIds = newData.ArtistIds,
            CardNameGuid = newData.CardNameGuid,
            CollectedList = [.. mergedItems.Values]
        };
    }
}
