using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Entities;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Mappers;

internal sealed class AddSetGroupCombinedArgToItrMapper : IAddSetGroupCombinedArgToItrMapper
{
    private readonly FinishCountsArgToItrMapper _finishCountsMapper;

    public AddSetGroupCombinedArgToItrMapper() : this(new FinishCountsArgToItrMapper())
    { }

    private AddSetGroupCombinedArgToItrMapper(FinishCountsArgToItrMapper finishCountsMapper) => _finishCountsMapper = finishCountsMapper;

    public async Task<IAddSetGroupToUserSetCardItrEntity> Map(IAddSetGroupToUserSetCardArgsEntity from)
    {
        IFinishCountsItrEntity counts = await _finishCountsMapper.Map(from.AddSetGroupToUserSetCard.Counts).ConfigureAwait(false);

        AddSetGroupToUserSetCardItrEntity itrEntity = new()
        {
            UserId = from.AuthUser.UserId,
            SetId = from.AddSetGroupToUserSetCard.SetId,
            SetGroupId = from.AddSetGroupToUserSetCard.SetGroupId,
            Collecting = from.AddSetGroupToUserSetCard.Collecting,
            Counts = counts,
            CollectingFinishes = from.AddSetGroupToUserSetCard.CollectingFinishes
        };

        return itrEntity;
    }
}
