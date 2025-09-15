using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Commands;

internal sealed class UserCardsCommandAdapter : IUserCardsCommandAdapter
{
    private readonly IUserCardsScribe _userCardsScribe;
    private readonly ILogger _logger;

    public UserCardsCommandAdapter(ILogger logger) : this(logger, new UserCardsScribe(logger))
    { }

    internal UserCardsCommandAdapter(ILogger logger, IUserCardsScribe userCardsScribe)
    {
        _logger = logger;
        _userCardsScribe = userCardsScribe;
    }

    public async Task<IOperationResponse<IUserCardCollectionItrEntity>> AddUserCardAsync(IUserCardCollectionItrEntity userCard)
    {
        UserCardItem userCardItem = MapToUserCardItem(userCard);
        OpResponse<UserCardItem> cosmosResponse = await _userCardsScribe
            .UpsertAsync(userCardItem)
            .ConfigureAwait(false);

        if (cosmosResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new UserCardsAdapterException($"Failed to add user card: {cosmosResponse.StatusCode}"));
        }

        IUserCardCollectionItrEntity resultEntity = MapToItrEntity(cosmosResponse.Value);
        return new SuccessOperationResponse<IUserCardCollectionItrEntity>(resultEntity);
    }

    private static UserCardItem MapToUserCardItem(IUserCardCollectionItrEntity userCard)
    {
        IEnumerable<CollectedItem> collectedItems = userCard.CollectedList.Select(MapToCollectedItem);

        return new UserCardItem
        {
            UserId = userCard.UserId,
            CardId = userCard.CardId,
            SetId = userCard.SetId,
            CollectedList = collectedItems
        };
    }

    private static CollectedItem MapToCollectedItem(ICollectedItemItrEntity collected)
    {
        return new CollectedItem
        {
            Finish = collected.Finish,
            Special = collected.Special,
            Count = collected.Count
        };
    }

    private static IUserCardCollectionItrEntity MapToItrEntity(UserCardItem userCardItem)
    {
        ICollection<ICollectedItemItrEntity> collectedEntities = [.. userCardItem.CollectedList.Select(MapToCollectedItrEntity)];

        return new UserCardCollectionItrEntity
        {
            UserId = userCardItem.UserId,
            CardId = userCardItem.CardId,
            SetId = userCardItem.SetId,
            CollectedList = collectedEntities
        };
    }

    private static ICollectedItemItrEntity MapToCollectedItrEntity(CollectedItem collectedItem)
    {
        return new CollectedItemItrEntity
        {
            Finish = collectedItem.Finish,
            Special = collectedItem.Special,
            Count = collectedItem.Count
        };
    }
}

internal sealed class UserCardCollectionItrEntity : IUserCardCollectionItrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<ICollectedItemItrEntity> CollectedList { get; init; }
}

internal sealed class CollectedItemItrEntity : ICollectedItemItrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
