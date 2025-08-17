using Lib.BlobStorage.Apis.Configurations.Ids;

namespace Lib.Scryfall.Ingestion.BlobStorage.Values;

internal sealed class SetIconBlobContainerName : BlobContainerName
{
    public override string AsSystemType() => "mtgsets";
}