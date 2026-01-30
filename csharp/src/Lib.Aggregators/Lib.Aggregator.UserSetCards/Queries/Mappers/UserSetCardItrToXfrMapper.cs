using System.Threading.Tasks;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Aggregator.UserSetCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal sealed class UserSetCardItrToXfrMapper : IUserSetCardItrToXfrMapper
{
    public Task<IUserSetCardGetXfrEntity> Map(IUserSetCardItrEntity userSetCard)
    {
        IUserSetCardGetXfrEntity xfrEntity = new UserSetCardXfrEntity
        {
            UserId = userSetCard.UserId,
            SetId = userSetCard.SetId
        };
        return Task.FromResult(xfrEntity);
    }
}
