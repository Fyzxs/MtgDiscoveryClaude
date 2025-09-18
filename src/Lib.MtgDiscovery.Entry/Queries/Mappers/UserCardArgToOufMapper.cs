using System.Collections.Generic;
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

internal sealed class UserCardItrEntity : IUserCardItrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<IUserCardDetailsItrEntity> CollectedList { get; init; }
}
