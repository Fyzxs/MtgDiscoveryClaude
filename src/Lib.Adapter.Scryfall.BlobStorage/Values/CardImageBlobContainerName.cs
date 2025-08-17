using Lib.BlobStorage.Apis.Configurations.Ids;

namespace Lib.Adapter.Scryfall.BlobStorage.Values;

internal sealed class CardImageBlobContainerName : BlobContainerName
{
    public override string AsSystemType() => "mtgcards";
}
