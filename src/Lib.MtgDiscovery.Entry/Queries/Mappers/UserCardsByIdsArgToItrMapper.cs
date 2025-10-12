using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class UserCardsByIdsArgToItrMapper : IUserCardsByIdsArgToItrMapper
{
    public Task<IUserCardsByIdsItrEntity> Map(IUserCardsByIdsArgEntity byIdsArgs)
    {
        return Task.FromResult<IUserCardsByIdsItrEntity>(new UserCardsByIdsItrEntity
        {
            UserId = byIdsArgs.UserId,
            CardIds = byIdsArgs.CardIds
        });
    }
}
