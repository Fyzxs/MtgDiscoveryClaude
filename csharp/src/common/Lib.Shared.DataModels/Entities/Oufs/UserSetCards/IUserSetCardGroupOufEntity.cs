namespace Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

public interface IUserSetCardGroupOufEntity
{
    IUserSetCardFinishGroupOufEntity NonFoil { get; }
    IUserSetCardFinishGroupOufEntity Foil { get; }
    IUserSetCardFinishGroupOufEntity Etched { get; }
}
