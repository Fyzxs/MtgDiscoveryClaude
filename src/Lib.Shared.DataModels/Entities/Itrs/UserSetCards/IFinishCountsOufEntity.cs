namespace Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

public interface IFinishCountsOufEntity
{
    int Total { get; }
    int NonFoil { get; }
    int Foil { get; }
    int Etched { get; }
}
