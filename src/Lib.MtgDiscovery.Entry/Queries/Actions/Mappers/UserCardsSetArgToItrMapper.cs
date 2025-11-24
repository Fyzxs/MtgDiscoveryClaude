using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class UserCardsSetArgToItrMapper : IUserCardsSetArgToItrMapper
{
    public Task<IUserCardsSetItrEntity> Map(IUserCardsBySetArgEntity bySetArgs)
    {
        return Task.FromResult<IUserCardsSetItrEntity>(new UserCardsSetItrEntity
        {
            UserId = bySetArgs.UserId,
            SetId = bySetArgs.SetId
        });
    }
}
