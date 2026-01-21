namespace Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

public interface IFinishCountsXfrEntity
{
    int Total { get; }
    int NonFoil { get; }
    int Foil { get; }
    int Etched { get; }
}
