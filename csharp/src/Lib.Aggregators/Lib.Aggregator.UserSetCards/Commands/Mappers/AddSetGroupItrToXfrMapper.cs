using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Commands.Mappers;

internal sealed class AddSetGroupItrToXfrMapper : IAddSetGroupItrToXfrMapper
{
    private readonly IFinishCountsItrToXfrMapper _finishCountsMapper;

    public AddSetGroupItrToXfrMapper() : this(new FinishCountsItrToXfrMapper())
    { }

    private AddSetGroupItrToXfrMapper(IFinishCountsItrToXfrMapper finishCountsMapper) => _finishCountsMapper = finishCountsMapper;

    public async Task<IAddSetGroupToUserSetCardXfrEntity> Map(IAddSetGroupToUserSetCardItrEntity source)
    {
        IFinishCountsXfrEntity counts = await _finishCountsMapper.Map(source.Counts).ConfigureAwait(false);

        return new AddSetGroupToUserSetCardXfrEntity
        {
            UserId = source.UserId,
            SetId = source.SetId,
            SetGroupId = source.SetGroupId,
            Collecting = source.Collecting,
            Counts = counts,
            CollectingFinishes = source.CollectingFinishes
        };
    }
}
