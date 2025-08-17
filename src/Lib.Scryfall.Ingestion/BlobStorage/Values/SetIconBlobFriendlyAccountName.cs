using Lib.BlobStorage.Apis.Configurations;

namespace Lib.Scryfall.Ingestion.BlobStorage.Values;

internal sealed class SetIconBlobFriendlyAccountName : BlobFriendlyAccountName
{
    public override string AsSystemType() => "MtgDiscovery";
}