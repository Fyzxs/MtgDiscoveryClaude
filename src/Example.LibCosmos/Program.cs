
using System.Threading.Tasks;
using Example.Core;
using Lib.Cosmos.Apis;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Example.LibCosmos;

internal class Program
{
    private static async Task Main(string[] args)
    {
        ExampleLibCosmosApplication app = new();
        await app.StartUp(args).ConfigureAwait(false);
    }
}

public sealed class ExampleLibCosmosApplication : ExampleApplication
{
    protected override async Task Execute()
    {
        await Task.Delay(0).ConfigureAwait(false);
        SetItemCosmosScribe gopher = new(NullLogger.Instance);
        _ = await gopher.UpsertAsync(new CosmosItem()).ConfigureAwait(false);
    }
}
internal sealed class SetItemCosmosScribe : CosmosScribe
{
    public SetItemCosmosScribe(ILogger logger) : base(new SetItemsCosmosContainer(logger))
    { }
}

internal sealed class SetItemCosmosGopher : CosmosGopher
{
    public SetItemCosmosGopher(ILogger logger) : base(new SetItemsCosmosContainer(logger))
    { }
}

internal sealed class SetItemsCosmosContainer : CosmosContainerAdapter
{
    public SetItemsCosmosContainer(ILogger logger) : base(logger, new SetItemsCosmosContainerDefinition(), new ServiceLocatorAuthCosmosConnectionConfig())
    { }
}

internal sealed class SetItemsCosmosContainerDefinition : ICosmosContainerDefinition
{
    public CosmosFriendlyAccountName FriendlyAccountName() => new MtgDiscoveryCosmosAccountName();

    public CosmosDatabaseName DatabaseName() => new MtgDiscoveryCosmosDatabaseName();

    public CosmosContainerName ContainerName() => new SetItemsCosmosContainerName();

    public CosmosPartitionKeyPath PartitionKeyPath() => new PartitionCosmosPartitionKeyPath();
}

internal sealed class MtgDiscoveryCosmosAccountName : CosmosFriendlyAccountName
{
    public override string AsSystemType() => "MtgDiscoveryAccount";
}
internal sealed class MtgDiscoveryCosmosDatabaseName : CosmosDatabaseName
{
    public override string AsSystemType() => "MtgDiscoveryV4";
}
internal sealed class SetItemsCosmosContainerName : CosmosContainerName
{
    public override string AsSystemType() => "SetItems";
}
