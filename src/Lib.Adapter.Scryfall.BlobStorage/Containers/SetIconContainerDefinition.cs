using Lib.Adapter.Scryfall.BlobStorage.Values;
using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Configurations.Ids;

namespace Lib.Adapter.Scryfall.BlobStorage.Containers;

internal sealed class SetIconContainerDefinition : IBlobContainerDefinition
{
    public BlobFriendlyAccountName FriendlyAccountName() => new SetIconBlobFriendlyAccountName();

    public BlobFriendlyContainerName FriendlyContainerName() => new SetIconBlobFriendlyContainerName();

    public BlobContainerName ContainerName() => new SetIconBlobContainerName();
}
