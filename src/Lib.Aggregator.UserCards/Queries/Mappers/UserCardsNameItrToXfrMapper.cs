using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

internal sealed class UserCardsNameItrToXfrMapper : IUserCardsNameItrToXfrMapper
{
    public Task<IUserCardsNameXfrEntity> Map(IUserCardsNameItrEntity source)
    {
        return Task.FromResult<IUserCardsNameXfrEntity>(new UserCardsNameXfrEntity
        {
            UserId = source.UserId,
            CardName = source.CardName
        });
    }
}
