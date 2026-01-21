using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class UserCardsSetItrEntity : IUserCardsSetItrEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
}
