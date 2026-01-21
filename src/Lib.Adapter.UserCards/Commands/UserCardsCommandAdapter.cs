using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Commands;

/// <summary>
/// Command adapter implementation for user card collection operations.
/// Handles write operations for adding cards to user collections in Cosmos DB.
///
/// Design Pattern: Command adapter following CQRS principles
/// - Focuses solely on command (write) operations
/// - Maps between ITR entities and Cosmos storage entities
/// - Delegates to UserCards Cosmos operators for data persistence
///
/// Entity Mapping Strategy:
/// - Input: IUserCardCollectionItrEntity (from domain/aggregator layers)
/// - Storage: UserCardItem (Cosmos document structure)
/// - Output: IUserCardCollectionItrEntity (mapped back from storage)
///
/// Error Handling:
/// All exceptions are wrapped in UserCardsAdapterException following
/// the established adapter exception pattern.
/// </summary>
internal sealed class UserCardsCommandAdapter : IUserCardsCommandAdapter
{
    private readonly IUserCardsScribe _userCardsScribe;
    private readonly ILogger _logger;

    public UserCardsCommandAdapter(ILogger logger)
    {
        _logger = logger;
        _userCardsScribe = new UserCardsScribe(logger);
    }

    public async Task<IOperationResponse<IUserCardCollectionItrEntity>> AddUserCardAsync(IUserCardCollectionItrEntity userCard)
    {
        try
        {
            UserCardItem userCardItem = MapToUserCardItem(userCard);
            Lib.Cosmos.Apis.Operators.OpResponse<UserCardItem> upsertResult = await _userCardsScribe.UpsertAsync(userCardItem).ConfigureAwait(false);

            if (upsertResult.IsNotSuccessful())
            {
                string message = $"Failed to upsert user card. UserId: {userCard.UserId}, CardId: {userCard.CardId}, StatusCode: {upsertResult.StatusCode}";
                UserCardsAdapterException exception = new(message);
                return new FailureOperationResponse<IUserCardCollectionItrEntity>(exception);
            }

            IUserCardCollectionItrEntity resultEntity = MapToItrEntity(upsertResult.Value);
            return new SuccessOperationResponse<IUserCardCollectionItrEntity>(resultEntity);
        }
        catch (System.Exception ex)
        {
            string message = $"Failed to add user card. UserId: {userCard.UserId}, CardId: {userCard.CardId}";
            _logger.LogError(ex, "Failed to add user card. UserId: {UserId}, CardId: {CardId}", userCard.UserId, userCard.CardId);
            throw new UserCardsAdapterException(message, ex);
        }
    }

    private static UserCardItem MapToUserCardItem(IUserCardCollectionItrEntity userCard)
    {
        System.Collections.Generic.IEnumerable<CollectedItem> collectedItems = userCard.CollectedList.Select(MapToCollectedItem);

        return new UserCardItem
        {
            UserId = userCard.UserId,
            CardId = userCard.CardId,
            SetId = userCard.SetId,
            CollectedList = collectedItems
        };
    }

    private static CollectedItem MapToCollectedItem(ICollectedCardItrEntity collected)
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
        System.Collections.Generic.IEnumerable<ICollectedCardItrEntity> collectedEntities = userCardItem.CollectedList.Select(MapToCollectedItrEntity);

        return new UserCardCollectionItrEntity
        {
            UserId = userCardItem.UserId,
            CardId = userCardItem.CardId,
            SetId = userCardItem.SetId,
            CollectedList = collectedEntities
        };
    }

    private static ICollectedCardItrEntity MapToCollectedItrEntity(CollectedItem collectedItem)
    {
        return new CollectedCardItrEntity
        {
            Finish = collectedItem.Finish,
            Special = collectedItem.Special,
            Count = collectedItem.Count
        };
    }
}

/// <summary>
/// Concrete implementation of IUserCardCollectionItrEntity.
/// Internal visibility following MicroObjects pattern where interfaces are public but implementations are internal.
/// </summary>
internal sealed class UserCardCollectionItrEntity : IUserCardCollectionItrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public System.Collections.Generic.IEnumerable<ICollectedCardItrEntity> CollectedList { get; init; }
}

/// <summary>
/// Concrete implementation of ICollectedCardItrEntity.
/// Internal visibility following MicroObjects pattern where interfaces are public but implementations are internal.
/// </summary>
internal sealed class CollectedCardItrEntity : ICollectedCardItrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
