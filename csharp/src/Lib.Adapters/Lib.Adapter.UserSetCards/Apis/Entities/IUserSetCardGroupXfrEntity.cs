namespace Lib.Adapter.UserSetCards.Apis.Entities;

public interface IUserSetCardGroupXfrEntity
{
    IUserSetCardFinishGroupXfrEntity NonFoil { get; }
    IUserSetCardFinishGroupXfrEntity Foil { get; }
    IUserSetCardFinishGroupXfrEntity Etched { get; }
}
