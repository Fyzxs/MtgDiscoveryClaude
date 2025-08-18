using Lib.Adapter.Scryfall.BlobStorage.Apis.Values;

namespace Lib.Adapter.Scryfall.BlobStorage.Values;

public sealed class CardImageData : ICardImageData
{
    private readonly string _side;
    private readonly string _imageType;
    private readonly byte[] _imageData;

    public CardImageData(string side, string imageType, byte[] imageData)
    {
        _side = side;
        _imageType = imageType;
        _imageData = imageData;
    }

    public string Side() => _side;
    public string ImageType() => _imageType;
    public byte[] ImageData() => _imageData;
}
