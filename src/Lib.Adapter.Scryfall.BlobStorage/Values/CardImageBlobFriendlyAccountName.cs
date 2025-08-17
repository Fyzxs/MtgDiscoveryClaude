using Lib.BlobStorage.Apis.Configurations;

namespace Lib.Adapter.Scryfall.BlobStorage.Values;

internal sealed class CardImageBlobFriendlyAccountName : BlobFriendlyAccountName
{
    public override string AsSystemType() => "MtgDiscovery";
}
