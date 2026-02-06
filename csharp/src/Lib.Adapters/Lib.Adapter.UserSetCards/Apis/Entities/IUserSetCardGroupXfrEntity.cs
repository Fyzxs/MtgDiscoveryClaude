using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.UserSetCards.Apis.Entities;

public interface IUserSetCardGroupXfrEntity : IXfrEntity
{
    IUserSetCardFinishGroupXfrEntity NonFoil { get; }
    IUserSetCardFinishGroupXfrEntity Foil { get; }
    IUserSetCardFinishGroupXfrEntity Etched { get; }
}
