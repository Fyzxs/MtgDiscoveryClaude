using Lib.BlobStorage.Apis.Configurations;

namespace Lib.Adapter.Scryfall.BlobStorage.Values;

internal sealed class SetIconBlobFriendlyContainerName : BlobFriendlyContainerName
{
    public override string AsSystemType() => "MtgSets";
}
