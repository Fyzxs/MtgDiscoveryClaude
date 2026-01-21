using System.Collections.Generic;
using System.Linq;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class UserCardItemMapper : IUserCardItemMapper
{
    private readonly ICollectedItemMapper _collectedItemMapper;

    public UserCardItemMapper() : this(new CollectedItemMapper())
    { }

    private UserCardItemMapper(ICollectedItemMapper collectedItemMapper) => _collectedItemMapper = collectedItemMapper;

    public UserCardItem Map(IUserCardCollectionItrEntity userCard)
    {
        IEnumerable<CollectedItem> collectedItems = userCard.CollectedList.Select(_collectedItemMapper.Map);

        return new UserCardItem
        {
            UserId = userCard.UserId,
            CardId = userCard.CardId,
            SetId = userCard.SetId,
            CollectedList = collectedItems
        };
    }
}
