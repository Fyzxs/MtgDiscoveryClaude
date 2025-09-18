using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class UserCardsSetArgToItrMapper : IUserCardsSetArgToItrMapper
{
    public Task<IUserCardsSetItrEntity> Map(IUserCardsSetArgEntity setArgs)
    {
        return Task.FromResult<IUserCardsSetItrEntity>(new UserCardsSetItrEntity
        {
            UserId = setArgs.UserId,
            SetId = setArgs.SetId
        });
    }
}
