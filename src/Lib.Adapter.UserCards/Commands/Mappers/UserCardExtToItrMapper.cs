using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserCards.Commands.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class UserCardExtToItrMapper : IUserCardExtToItrMapper
{
    private readonly IUserCardDetailsExtToItrMapper _collectedItemMapper;

    public UserCardExtToItrMapper() : this(new UserCardDetailsExtToItrMapper())
    { }

    private UserCardExtToItrMapper(IUserCardDetailsExtToItrMapper collectedItemMapper) => _collectedItemMapper = collectedItemMapper;

    public async Task<IUserCardItrEntity> Map(UserCardExtEntity userCardItem)
    {
        List<IUserCardDetailsItrEntity> collectedEntities = [];
        foreach (UserCardDetailsExtEntity item in userCardItem.CollectedList)
        {
            IUserCardDetailsItrEntity mapped = await _collectedItemMapper.Map(item).ConfigureAwait(false);
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
