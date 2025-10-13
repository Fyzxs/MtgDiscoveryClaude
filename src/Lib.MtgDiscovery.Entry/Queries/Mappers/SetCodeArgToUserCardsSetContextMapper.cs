using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class SetCodeArgToUserCardsSetContextMapper : ISetCodeArgToUserCardsSetContextMapper
{
    public Task<IUserCardsSetItrEntity> Map(ISetCodeArgEntity setCode, List<CardItemOutEntity> outEntities)
    {
        // All cards in outEntities are from the same set, extract SetId UUID from first card
        CardItemOutEntity firstCard = outEntities.FirstOrDefault();

        if (firstCard is null || string.IsNullOrEmpty(firstCard.SetId))
        {
            return Task.FromResult<IUserCardsSetItrEntity>(null);
        }

        IUserCardsSetItrEntity context = new UserCardsSetItrEntity
        {
            UserId = setCode.UserId,
            SetId = firstCard.SetId
        };

        return Task.FromResult(context);
    }
}
