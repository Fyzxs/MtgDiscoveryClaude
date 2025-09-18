using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Queries.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

internal sealed class UserCardsSetItrToXfrMapper : IUserCardsSetItrToXfrMapper
{
    public Task<IUserCardsSetXfrEntity> Map(IUserCardsSetItrEntity source)
    {
        return Task.FromResult<IUserCardsSetXfrEntity>(new UserCardsSetXfrEntity
        {
            UserId = source.UserId,
            SetId = source.SetId
        });
    }
}