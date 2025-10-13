using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Entities;

internal sealed class UserSetCardGroupOufEntity : IUserSetCardGroupOufEntity
{
    public IUserSetCardFinishGroupOufEntity NonFoil { get; init; }
    public IUserSetCardFinishGroupOufEntity Foil { get; init; }
    public IUserSetCardFinishGroupOufEntity Etched { get; init; }
}
