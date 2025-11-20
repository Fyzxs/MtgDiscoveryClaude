namespace Lib.Shared.DataModels.Entities.Args.UserSetCards;

public interface IFinishCountsArgEntity
{
    int Total { get; }
    int NonFoil { get; }
    int Foil { get; }
    int Etched { get; }
}
