using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Entities;
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

internal sealed class UserSetCardItrEntity : IUserSetCardItrEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
}
