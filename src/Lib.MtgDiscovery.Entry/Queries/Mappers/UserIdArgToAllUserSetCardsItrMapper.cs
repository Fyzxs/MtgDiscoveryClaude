using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class UserIdArgToAllUserSetCardsItrMapper : IUserIdArgToAllUserSetCardsItrMapper
{
    public Task<IAllUserSetCardsItrEntity> Map(IUserIdArgEntity arg)
    {
        IAllUserSetCardsItrEntity itrEntity = new AllUserSetCardsItrEntity
        {
            UserId = arg.UserId
        };
        return Task.FromResult(itrEntity);
    }
}
