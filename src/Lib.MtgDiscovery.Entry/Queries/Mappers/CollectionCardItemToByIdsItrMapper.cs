using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CollectionCardItemToByIdsItrMapper : ICollectionCardItemToByIdsItrMapper
{
    public Task<IUserCardsByIdsItrEntity> Map(List<CardItemOutEntity> cards, IUserIdArgEntity argEntity)
    {
        List<string> cardIds = cards.Select(c => c.Id).ToList();

        IUserCardsByIdsItrEntity result = new UserCardsByIdsItrEntity
        {
            UserId = argEntity.UserId,
            CardIds = cardIds
        };

        return Task.FromResult(result);
    }
}
