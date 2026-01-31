using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class UserSetCardItrEntity : IUserSetCardItrEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
}
