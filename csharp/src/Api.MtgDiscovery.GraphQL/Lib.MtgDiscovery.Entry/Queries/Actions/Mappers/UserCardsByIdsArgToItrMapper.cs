using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

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
