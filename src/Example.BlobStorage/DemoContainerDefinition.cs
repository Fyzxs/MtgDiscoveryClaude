using Lib.BlobStorage.Apis.Configurations;
using Lib.BlobStorage.Apis.Configurations.Ids;

namespace Example.BlobStorage;

public interface IDemoContainerDefinition : IBlobContainerDefinition;

internal sealed class DemoContainerDefinition : IDemoContainerDefinition
{
    public BlobFriendlyAccountName FriendlyAccountName() => new DemoBlobFriendlyAccountName();

    public BlobFriendlyContainerName FriendlyContainerName() => new DemoBlobFriendlyContainerName();

    public BlobAccountName AccountName() => new DemoBlobAccountName();

    public BlobContainerName ContainerName() => new DemoBlobContainerName();
}

internal sealed class DemoBlobFriendlyAccountName : BlobFriendlyAccountName
{
    public override string AsSystemType() => "ExampleStorage";
}

internal sealed class DemoBlobFriendlyContainerName : BlobFriendlyContainerName
{
    public override string AsSystemType() => "DemoContainer";
}

internal sealed class DemoBlobAccountName : BlobAccountName
{
    public override string AsSystemType() => "devstoreaccount1";
}

internal sealed class DemoBlobContainerName : BlobContainerName
{
    public override string AsSystemType() => "demo-container";
}
