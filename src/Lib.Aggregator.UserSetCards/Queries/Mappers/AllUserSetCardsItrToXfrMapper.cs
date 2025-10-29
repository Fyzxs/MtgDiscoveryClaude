using System.Threading.Tasks;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Aggregator.UserSetCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface IAllUserSetCardsItrToXfrMapper
{
    Task<IAllUserSetCardsXfrEntity> Map(IAllUserSetCardsItrEntity itr);
}

internal sealed class AllUserSetCardsItrToXfrMapper : IAllUserSetCardsItrToXfrMapper
{
    public Task<IAllUserSetCardsXfrEntity> Map(IAllUserSetCardsItrEntity itr)
    {
        IAllUserSetCardsXfrEntity xfrEntity = new AllUserSetCardsXfrEntity
        {
            UserId = itr.UserId
        };
        return Task.FromResult(xfrEntity);
    }
}
