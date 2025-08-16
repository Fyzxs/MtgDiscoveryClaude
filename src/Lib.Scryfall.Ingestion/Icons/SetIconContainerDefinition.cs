using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Configurations.Ids;

namespace Lib.Scryfall.Ingestion.Icons;

internal interface ISetIconContainerDefinition : IBlobContainerDefinition { }

internal sealed class SetIconContainerDefinition : ISetIconContainerDefinition
{
    public BlobFriendlyAccountName FriendlyAccountName() => new SetIconBlobFriendlyAccountName();

    public BlobFriendlyContainerName FriendlyContainerName() => new SetIconBlobFriendlyContainerName();

    public BlobAccountName AccountName() => new SetIconBlobAccountName();

    public BlobContainerName ContainerName() => new SetIconBlobContainerName();
}

internal sealed class SetIconBlobFriendlyAccountName : BlobFriendlyAccountName
{
    public override string AsSystemType() => "MtgDiscovery";
}

internal sealed class SetIconBlobFriendlyContainerName : BlobFriendlyContainerName
{
    public override string AsSystemType() => "MtgSets";
}

internal sealed class SetIconBlobAccountName : BlobAccountName
{
    public override string AsSystemType() => "mtgdiscoverystorage";
}

internal sealed class SetIconBlobContainerName : BlobContainerName
{
    public override string AsSystemType() => "mtgsets";
}