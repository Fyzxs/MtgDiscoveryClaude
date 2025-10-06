namespace Lib.Aggregator.UserSetCards.Entities;

public interface IUserSetCardGroupOufEntity
{
    IUserSetCardFinishGroupOufEntity NonFoil { get; }
    IUserSetCardFinishGroupOufEntity Foil { get; }
    IUserSetCardFinishGroupOufEntity Etched { get; }
}
