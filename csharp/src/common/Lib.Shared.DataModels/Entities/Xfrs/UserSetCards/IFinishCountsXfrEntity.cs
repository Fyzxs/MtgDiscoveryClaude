using Lib.Shared.DataModels.Abstractions;

namespace Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

public interface IFinishCountsXfrEntity : IXfrEntity
{
    int Total { get; }
    int NonFoil { get; }
    int Foil { get; }
    int Etched { get; }
}
