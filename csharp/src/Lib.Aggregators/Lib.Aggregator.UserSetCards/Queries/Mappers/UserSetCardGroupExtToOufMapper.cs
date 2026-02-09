using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Aggregator.UserSetCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal sealed class UserSetCardGroupExtToOufMapper : IUserSetCardGroupExtToOufMapper
{
    private readonly IUserSetCardFinishGroupExtToOufMapper _finishMapper;

    public UserSetCardGroupExtToOufMapper() : this(new UserSetCardFinishGroupExtToOufMapper())
    { }

    private UserSetCardGroupExtToOufMapper(IUserSetCardFinishGroupExtToOufMapper finishMapper) => _finishMapper = finishMapper;

    public async Task<IUserSetCardGroupOufEntity> Map(UserSetCardGroupExtEntity groupExt)
    {
        return new UserSetCardGroupOufEntity
        {
            NonFoil = await _finishMapper.Map(groupExt.NonFoil).ConfigureAwait(false),
            Foil = await _finishMapper.Map(groupExt.Foil).ConfigureAwait(false),
            Etched = await _finishMapper.Map(groupExt.Etched).ConfigureAwait(false)
        };
    }
}
