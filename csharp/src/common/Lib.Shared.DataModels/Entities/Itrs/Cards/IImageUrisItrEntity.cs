namespace Lib.Shared.DataModels.Entities.Itrs.Cards;

public interface IImageUrisItrEntity
{
    string Small { get; }
    string Normal { get; }
    string Large { get; }
    string Png { get; }
    string ArtCrop { get; }
    string BorderCrop { get; }
}
