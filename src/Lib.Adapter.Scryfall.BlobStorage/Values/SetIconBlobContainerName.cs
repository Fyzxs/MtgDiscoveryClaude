using Lib.BlobStorage.Apis.Configurations.Ids;

namespace Lib.Adapter.Scryfall.BlobStorage.Values;

internal sealed class SetIconBlobContainerName : BlobContainerName
{
    public override string AsSystemType() => "mtgsets";
}
