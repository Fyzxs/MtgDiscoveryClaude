using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Scryfall.Shared.Internals;

internal sealed class ImageUrisItrEntity : IImageUrisItrEntity
{
    private readonly dynamic _data;

    public ImageUrisItrEntity(dynamic data) => _data = data;

    public string Small => _data?.small;
    public string Normal => _data?.normal;
    public string Large => _data?.large;
    public string Png => _data?.png;
    public string ArtCrop => _data?.art_crop;
    public string BorderCrop => _data?.border_crop;
}
