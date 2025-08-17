using Lib.BlobStorage.Apis.Configurations;

namespace Lib.Scryfall.Ingestion.BlobStorage.Values;

internal sealed class SetIconBlobFriendlyContainerName : BlobFriendlyContainerName
{
    public override string AsSystemType() => "MtgSets";
}