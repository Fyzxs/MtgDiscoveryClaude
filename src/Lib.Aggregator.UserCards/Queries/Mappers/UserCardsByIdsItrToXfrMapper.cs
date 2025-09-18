using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

internal sealed class UserCardsByIdsItrToXfrMapper : IUserCardsByIdsItrToXfrMapper
{
    public Task<IUserCardsByIdsXfrEntity> Map(IUserCardsByIdsItrEntity source)
    {
        return Task.FromResult<IUserCardsByIdsXfrEntity>(new UserCardsByIdsXfrEntity
        {
            UserId = source.UserId,
            CardIds = source.CardIds
        });
    }
}
