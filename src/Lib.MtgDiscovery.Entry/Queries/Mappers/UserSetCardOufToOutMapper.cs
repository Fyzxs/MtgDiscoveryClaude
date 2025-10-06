using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.Shared.DataModels.Entities.Outs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class UserSetCardOufToOutMapper : IUserSetCardOufToOutMapper
{
    public Task<UserSetCardOutEntity> Map(IUserSetCardOufEntity oufEntity)
    {
        Dictionary<string, UserSetCardGroupOutEntity> groups = oufEntity.Groups
            .ToDictionary(
                kvp => kvp.Key,
                kvp => new UserSetCardGroupOutEntity
                {
                    NonFoil = new UserSetCardFinishGroupOutEntity { Cards = kvp.Value.NonFoil.Cards },
                    Foil = new UserSetCardFinishGroupOutEntity { Cards = kvp.Value.Foil.Cards },
                    Etched = new UserSetCardFinishGroupOutEntity { Cards = kvp.Value.Etched.Cards }
                });

        return Task.FromResult(new UserSetCardOutEntity
        {
            UserId = oufEntity.UserId,
            SetId = oufEntity.SetId,
            TotalCards = oufEntity.TotalCards,
            UniqueCards = oufEntity.UniqueCards,
            Groups = groups,
            GroupsCollecting = oufEntity.GroupsCollecting
        });
    }
}
