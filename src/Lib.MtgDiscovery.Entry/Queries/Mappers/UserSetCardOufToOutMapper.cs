using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class UserSetCardOufToOutMapper : IUserSetCardOufToOutMapper
{
    public Task<UserSetCardOutEntity> Map(IUserSetCardOufEntity oufEntity)
    {
        List<UserSetCardRarityGroupOutEntity> groups = [.. oufEntity.Groups
            .Select(kvp => new UserSetCardRarityGroupOutEntity
            {
                Rarity = kvp.Key,
                Group = new UserSetCardGroupOutEntity
                {
                    NonFoil = new UserSetCardFinishGroupOutEntity { Cards = kvp.Value.NonFoil.Cards },
                    Foil = new UserSetCardFinishGroupOutEntity { Cards = kvp.Value.Foil.Cards },
                    Etched = new UserSetCardFinishGroupOutEntity { Cards = kvp.Value.Etched.Cards }
                }
            })];

        List<UserSetCardCollectingOutEntity> collecting = [.. oufEntity.Collecting
            .Select(c => new UserSetCardCollectingOutEntity
            {
                SetGroupId = c.SetGroupId,
                Collecting = c.Collecting,
                Count = c.Count
            })];

        return Task.FromResult(new UserSetCardOutEntity
        {
            UserId = oufEntity.UserId,
            SetId = oufEntity.SetId,
            TotalCards = oufEntity.TotalCards,
            UniqueCards = oufEntity.UniqueCards,
            Groups = groups,
            Collecting = collecting
        });
    }
}
