using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserSetCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal sealed class UserSetCardFinishGroupExtToItrMapper : IUserSetCardFinishGroupExtToItrMapper
{
    public Task<IUserSetCardFinishGroupOufEntity> Map(UserSetCardFinishGroupExtEntity finishExt)
    {
        IUserSetCardFinishGroupOufEntity result = new UserSetCardFinishGroupOufEntity
        {
            Cards = [.. finishExt.Cards]
        };
        return Task.FromResult(result);
    }
}
