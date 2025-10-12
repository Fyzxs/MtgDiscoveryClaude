using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class UserCardsNameItrEntity : IUserCardsNameItrEntity
{
    public string UserId { get; init; }
    public string CardName { get; init; }
}
