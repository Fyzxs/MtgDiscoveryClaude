using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class UserCardArgToItrMapper : IUserCardArgToItrMapper
{
    public Task<IUserCardItrEntity> Map(IUserCardArgEntity userCardArgs)
    {
        return Task.FromResult<IUserCardItrEntity>(new UserCardItrEntity
        {
            UserId = userCardArgs.UserId,
            CardId = userCardArgs.CardId,
            SetId = null,
            CollectedList = []
        });
    }
}
