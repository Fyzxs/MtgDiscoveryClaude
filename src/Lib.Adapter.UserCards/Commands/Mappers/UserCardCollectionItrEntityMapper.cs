using System.Collections.Generic;
using System.Linq;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal interface IUserCardCollectionItrEntityMapper
{
    IUserCardCollectionItrEntity Map(UserCardItem userCardItem);
}

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

internal sealed class UserCardCollectionItrEntity : IUserCardCollectionItrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<ICollectedItemItrEntity> CollectedList { get; init; }
}
