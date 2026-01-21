using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserSetCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal sealed class UserSetCardGroupExtToItrMapper : IUserSetCardGroupExtToItrMapper
{
    private readonly IUserSetCardFinishGroupExtToItrMapper _finishMapper;

    public UserSetCardGroupExtToItrMapper() : this(new UserSetCardFinishGroupExtToItrMapper())
    { }

    private UserSetCardGroupExtToItrMapper(IUserSetCardFinishGroupExtToItrMapper finishMapper) => _finishMapper = finishMapper;

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
