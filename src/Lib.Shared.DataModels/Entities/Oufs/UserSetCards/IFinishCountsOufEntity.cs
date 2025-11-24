namespace Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

public interface IFinishCountsOufEntity
{
    int Total { get; }
    int NonFoil { get; }
    int Foil { get; }
    int Etched { get; }
}
