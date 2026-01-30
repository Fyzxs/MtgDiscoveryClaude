using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class CollectionCardItemToWishlistByIdsItrMapper : ICollectionCardItemToWishlistByIdsItrMapper
{
    public Task<IUserWishlistCardsByIdsItrEntity> Map(List<CardItemOutEntity> cards, IUserIdArgEntity argEntity)
    {
        List<string> cardIds = [.. cards.Select(c => c.Id)];

        IUserWishlistCardsByIdsItrEntity result = new UserWishlistCardsByIdsItrEntity
        {
            UserId = argEntity.UserId,
            CardIds = cardIds
        };

        return Task.FromResult(result);
    }
}
