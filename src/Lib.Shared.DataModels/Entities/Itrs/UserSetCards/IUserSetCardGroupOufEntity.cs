namespace Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

public interface IUserSetCardGroupOufEntity
{
    IUserSetCardFinishGroupOufEntity NonFoil { get; }
    IUserSetCardFinishGroupOufEntity Foil { get; }
    IUserSetCardFinishGroupOufEntity Etched { get; }
}
