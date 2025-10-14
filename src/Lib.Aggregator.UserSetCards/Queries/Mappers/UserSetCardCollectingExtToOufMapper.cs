using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserSetCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal sealed class UserSetCardCollectingExtToOufMapper : IUserSetCardCollectingExtToOufMapper
{
    public Task<IUserSetCardCollectingOufEntity> Map(UserSetCardCollectingExtEntity source)
    {
        return Task.FromResult<IUserSetCardCollectingOufEntity>(new UserSetCardCollectingOufEntity
        {
            SetGroupId = source.SetGroupId,
            Collecting = source.Collecting,
            Count = source.Count
        });
    }
}
