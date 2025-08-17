using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Configurations.Ids;
using Lib.Scryfall.Ingestion.BlobStorage.Values;

namespace Lib.Scryfall.Ingestion.BlobStorage.Containers;

internal sealed class SetIconContainerDefinition : IBlobContainerDefinition
{
    public BlobFriendlyAccountName FriendlyAccountName() => new SetIconBlobFriendlyAccountName();

    public BlobFriendlyContainerName FriendlyContainerName() => new SetIconBlobFriendlyContainerName();

    public BlobContainerName ContainerName() => new SetIconBlobContainerName();
}
