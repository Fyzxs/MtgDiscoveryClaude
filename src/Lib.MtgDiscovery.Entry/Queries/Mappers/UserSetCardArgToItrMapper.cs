using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class UserSetCardArgToItrMapper : IUserSetCardArgToItrMapper
{
    public Task<IUserSetCardItrEntity> Map(IUserSetCardArgEntity userSetCardArgs)
    {
        return Task.FromResult<IUserSetCardItrEntity>(new UserSetCardItrEntity
        {
            UserId = userSetCardArgs.UserId,
            SetId = userSetCardArgs.SetId
        });
    }
}
