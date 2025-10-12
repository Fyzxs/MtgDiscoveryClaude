namespace Lib.Aggregator.UserSetCards.Entities;

internal sealed class UserSetCardGroupOufEntity : IUserSetCardGroupOufEntity
{
    public IUserSetCardFinishGroupOufEntity NonFoil { get; init; }
    public IUserSetCardFinishGroupOufEntity Foil { get; init; }
    public IUserSetCardFinishGroupOufEntity Etched { get; init; }
}
