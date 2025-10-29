using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IAllUserSetCardsArgToItrMapper
{
    Task<IAllUserSetCardsItrEntity> Map(IAllUserSetCardsArgEntity arg);
}

internal sealed class AllUserSetCardsArgToItrMapper : IAllUserSetCardsArgToItrMapper
{
    public Task<IAllUserSetCardsItrEntity> Map(IAllUserSetCardsArgEntity arg)
    {
        IAllUserSetCardsItrEntity itrEntity = new AllUserSetCardsItrEntity
        {
            UserId = arg.UserId
        };
        return Task.FromResult(itrEntity);
    }
}
