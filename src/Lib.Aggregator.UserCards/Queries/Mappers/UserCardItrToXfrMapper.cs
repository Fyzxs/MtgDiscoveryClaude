using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

internal sealed class UserCardItrToXfrMapper : IUserCardItrToXfrMapper
{
    public Task<IUserCardXfrEntity> Map(IUserCardItrEntity source)
    {
        return Task.FromResult<IUserCardXfrEntity>(new UserCardXfrEntity
        {
            UserId = source.UserId,
            CardId = source.CardId
        });
    }
}
