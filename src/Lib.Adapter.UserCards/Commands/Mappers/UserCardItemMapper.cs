using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public async Task<UserCardItem> Map(IUserCardCollectionItrEntity userCard)
    {
        List<CollectedItem> collectedItems = [];
        foreach (ICollectedItemItrEntity item in userCard.CollectedList)
        {
            CollectedItem mapped = await _collectedItemMapper.Map(item).ConfigureAwait(false);
            collectedItems.Add(mapped);
        }

        return new UserCardItem
        {
            UserId = userCard.UserId,
            CardId = userCard.CardId,
            SetId = userCard.SetId,
            CollectedList = collectedItems
        };
    }
}
