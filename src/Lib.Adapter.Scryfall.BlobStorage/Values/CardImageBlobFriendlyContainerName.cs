using Lib.BlobStorage.Apis.Configurations;

namespace Lib.Adapter.Scryfall.BlobStorage.Values;

internal sealed class CardImageBlobFriendlyContainerName : BlobFriendlyContainerName
{
    public override string AsSystemType() => "MtgCards";
}
