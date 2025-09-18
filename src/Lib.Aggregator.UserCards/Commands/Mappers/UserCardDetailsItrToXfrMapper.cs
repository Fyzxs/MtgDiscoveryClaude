using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

internal sealed class UserCardDetailsItrToXfrMapper : IUserCardDetailsItrToXfrMapper
{
    public Task<IUserCardDetailsXfrEntity> Map(IUserCardDetailsItrEntity source)
    {
        return Task.FromResult<IUserCardDetailsXfrEntity>(new UserCardDetailsXfrEntity
        {
            Finish = source.Finish,
            Special = source.Special,
            Count = source.Count
        });
    }
}
