using System.Collections.Generic;
using System.Linq;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Commands.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class UserCardCollectionItrEntityMapper : IUserCardCollectionItrEntityMapper
{
    private readonly ICollectedItemItrEntityMapper _collectedItemMapper;

    public UserCardCollectionItrEntityMapper() : this(new CollectedItemItrEntityMapper())
    { }

    private UserCardCollectionItrEntityMapper(ICollectedItemItrEntityMapper collectedItemMapper) => _collectedItemMapper = collectedItemMapper;

    public IUserCardCollectionItrEntity Map(UserCardItem userCardItem)
    {
        ICollection<ICollectedItemItrEntity> collectedEntities = [.. userCardItem.CollectedList.Select(_collectedItemMapper.Map)];

        return new UserCardCollectionItrEntity
        {
            UserId = userCardItem.UserId,
            CardId = userCardItem.CardId,
            SetId = userCardItem.SetId,
            CollectedList = collectedEntities
        };
    }
}
