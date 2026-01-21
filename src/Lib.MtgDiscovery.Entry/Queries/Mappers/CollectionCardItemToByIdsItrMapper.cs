using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CollectionCardItemToByIdsItrMapper : ICollectionCardItemToByIdsItrMapper
{
    public Task<IUserCardsByIdsItrEntity> Map(List<CardItemOutEntity> cards, IUserIdArgEntity argEntity)
    {
        List<string> cardIds = [.. cards.Select(c => c.Id)];

        IUserCardsByIdsItrEntity result = new UserCardsByIdsItrEntity
        {
            UserId = argEntity.UserId,
            CardIds = cardIds
        };

        return Task.FromResult(result);
    }
}
