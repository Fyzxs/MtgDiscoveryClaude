namespace Lib.Adapter.Scryfall.BlobStorage.Apis.Values;

public interface ICardImageData
{
    string Side();
    string ImageType();
    byte[] ImageData();
}