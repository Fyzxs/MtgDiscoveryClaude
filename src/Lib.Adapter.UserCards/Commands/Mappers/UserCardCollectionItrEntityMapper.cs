using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
using Lib.Adapter.UserCards.Commands.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class UserCardCollectionItrEntityMapper : IUserCardCollectionItrEntityMapper
{
    private readonly ICollectedItemItrEntityMapper _collectedItemMapper;

    public UserCardCollectionItrEntityMapper() : this(new CollectedItemItrEntityMapper())
    { }

    private UserCardCollectionItrEntityMapper(ICollectedItemItrEntityMapper collectedItemMapper) => _collectedItemMapper = collectedItemMapper;

    public async Task<IUserCardCollectionItrEntity> Map(UserCardItem userCardItem)
    {
        List<ICollectedItemItrEntity> collectedEntities = [];
        foreach (CollectedItem item in userCardItem.CollectedList)
        {
            ICollectedItemItrEntity mapped = await _collectedItemMapper.Map(item).ConfigureAwait(false);
            collectedEntities.Add(mapped);
        }

        return new UserCardCollectionItrEntity
        {
            UserId = userCardItem.UserId,
            CardId = userCardItem.CardId,
            SetId = userCardItem.SetId,
            CollectedList = collectedEntities
        };
    }
}
