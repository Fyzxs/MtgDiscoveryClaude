using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserSetCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal sealed class UserSetCardCollectingExtToOufMapper : IUserSetCardCollectingExtToOufMapper
{
    private readonly IFinishCountsExtToOufMapper _finishCountsMapper;

    public UserSetCardCollectingExtToOufMapper() : this(new FinishCountsExtToOufMapper())
    { }

    private UserSetCardCollectingExtToOufMapper(IFinishCountsExtToOufMapper finishCountsMapper) => _finishCountsMapper = finishCountsMapper;

    public async Task<IUserSetCardCollectingOufEntity> Map(UserSetCardCollectingExtEntity source)
    {
        IFinishCountsOufEntity counts = await _finishCountsMapper.Map(source.Counts).ConfigureAwait(false);

        return new UserSetCardCollectingOufEntity
        {
            SetGroupId = source.SetGroupId,
            Collecting = source.Collecting,
            Counts = counts,
            CollectingFinishes = source.CollectingFinishes
        };
    }
}
