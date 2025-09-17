using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class UserCardItrToExtapper : IUserCardItrToExtapper
{
    private readonly IUserCardDetailsItrToExtMapper _collectedItemMapper;

    public UserCardItrToExtapper() : this(new UserCardDetailsItrToExtMapper())
    { }

    private UserCardItrToExtapper(IUserCardDetailsItrToExtMapper collectedItemMapper) => _collectedItemMapper = collectedItemMapper;

    public async Task<UserCardExtEntity> Map(IUserCardItrEntity userCard)
    {
        List<UserCardDetailsExtEntity> collectedItems = [];
        foreach (IUserCardDetailsItrEntity item in userCard.CollectedList)
        {
            UserCardDetailsExtEntity mapped = await _collectedItemMapper.Map(item).ConfigureAwait(false);
            collectedItems.Add(mapped);
        }

        return new UserCardExtEntity
        {
            UserId = userCard.UserId,
            CardId = userCard.CardId,
            SetId = userCard.SetId,
            CollectedList = collectedItems
        };
    }
}
