using Lib.Adapter.Scryfall.BlobStorage.Values;
using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Configurations.Ids;

namespace Lib.Adapter.Scryfall.BlobStorage.Containers;

internal sealed class CardImageContainerDefinition : IBlobContainerDefinition
{
    public BlobFriendlyAccountName FriendlyAccountName() => new CardImageBlobFriendlyAccountName();

    public BlobFriendlyContainerName FriendlyContainerName() => new CardImageBlobFriendlyContainerName();

    public BlobContainerName ContainerName() => new CardImageBlobContainerName();
}