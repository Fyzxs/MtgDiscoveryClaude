using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class AllUserSetCardsItrEntity : IAllUserSetCardsItrEntity
{
    public string UserId { get; init; }
}
