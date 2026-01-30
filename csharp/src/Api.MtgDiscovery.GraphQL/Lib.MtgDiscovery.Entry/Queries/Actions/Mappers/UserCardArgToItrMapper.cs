using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class UserCardArgToItrMapper : IUserCardArgToItrMapper
{
    public Task<IUserCardItrEntity> Map(IUserCardArgEntity userCardArgs)
    {
        return Task.FromResult<IUserCardItrEntity>(new UserCardItrEntity
        {
            UserId = userCardArgs.UserId,
            CardId = userCardArgs.CardId,
            SetId = null,
            Details = null
        });
    }
}
