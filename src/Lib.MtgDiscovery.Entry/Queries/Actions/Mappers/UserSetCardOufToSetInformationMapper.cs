using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class UserSetCardOufToSetInformationMapper : IUserSetCardOufToSetInformationMapper
{
    private readonly IFinishCountsOufToOutMapper _finishCountsMapper;

    public UserSetCardOufToSetInformationMapper() : this(new FinishCountsOufToOutMapper())
    { }

    private UserSetCardOufToSetInformationMapper(IFinishCountsOufToOutMapper finishCountsMapper) => _finishCountsMapper = finishCountsMapper;

    public async Task<SetInformationOutEntity> Map(IUserSetCardOufEntity oufEntity)
    {
        List<UserSetCardCollectionGroupOutEntity> groups = [.. oufEntity.Groups
            .Select(kvp => new UserSetCardCollectionGroupOutEntity
            {
                SetGroupId = kvp.Key,
                Group = new UserSetCardGroupOutEntity
                {
                    NonFoil = new UserSetCardFinishGroupOutEntity { Cards = kvp.Value.NonFoil.Cards },
                    Foil = new UserSetCardFinishGroupOutEntity { Cards = kvp.Value.Foil.Cards },
                    Etched = new UserSetCardFinishGroupOutEntity { Cards = kvp.Value.Etched.Cards }
                }
            })];

        UserSetCardCollectingOutEntity[] collectingArray = await Task.WhenAll(
            oufEntity.Collecting.Select(async c => new UserSetCardCollectingOutEntity
            {
                SetGroupId = c.SetGroupId,
                Collecting = c.Collecting,
                Counts = await _finishCountsMapper.Map(c.Counts).ConfigureAwait(false),
                CollectingFinishes = c.CollectingFinishes
            })
        ).ConfigureAwait(false);

        return new SetInformationOutEntity
        {
            TotalCards = oufEntity.TotalCards,
            UniqueCards = oufEntity.UniqueCards,
            Groups = groups,
            Collecting = [.. collectingArray]
        };
    }
}
