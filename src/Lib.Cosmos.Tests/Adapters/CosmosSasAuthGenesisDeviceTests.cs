using System.Threading.Tasks;
using Lib.Cosmos.Adapters;
using Lib.Cosmos.Tests.Fakes;

namespace Lib.Cosmos.Tests.Adapters;

[TestClass]
public sealed class CosmosSasAuthGenesisDeviceTests
{
    [TestMethod, TestCategory("unit")]
    public async Task LiveLongAndProsper_WithDatabaseLevelThroughput_CreatesDatabaseWithThroughputAndContainerWithout()
    {
        // Arrange
        int expectedDatabaseAutoscaleMax = 4000;
        string expectedContainerName = "TestContainer";
        string expectedPartitionKeyPath = "/partitionKey";
        int? expectedTtl = 3600;

        CosmosContainerConfigFake containerConfigFake = new()
        {
            TimeToLiveResult = new CosmosContainerTimeToLiveFake(expectedTtl)
        };

        CosmosThroughputModeFake throughputModeFake = new() { IsDatabaseSharedResult = true };

        CosmosDatabaseConfigFake databaseConfigFake = new()
        {
            ThroughputModeResult = throughputModeFake,
            AutoscaleMaxResult = new CosmosAutoscaleMaxFake(expectedDatabaseAutoscaleMax),
            ContainerConfigResult = containerConfigFake
        };

        CosmosContainerDefinitionFake containerDefinitionFake = new()
        {
            ContainerNameResult = new CosmosContainerNameFake(expectedContainerName),
            PartitionKeyPathResult = new CosmosPartitionKeyPathFake(expectedPartitionKeyPath)
        };

        CosmosConnectionConvenienceFake connectionConvenienceFake = new()
        {
            DatabaseConfigResult = databaseConfigFake
        };

        DatabaseFake databaseFake = new();

        DatabaseResponseFake databaseResponseFake = new()
        {
            DatabaseResult = databaseFake
        };

        CosmosGenesisClientAdapterFake genesisClientAdapterFake = new()
        {
            CreateDatabaseIfNotExistsAsyncResult = databaseResponseFake
        };

        CosmosSasAuthGenesisDevice subject = new(containerDefinitionFake, connectionConvenienceFake);

        // Act
        await subject.LiveLongAndProsper(genesisClientAdapterFake).ConfigureAwait(false);

        // Assert - Database creation
        genesisClientAdapterFake.CreateDatabaseIfNotExistsAsyncInvokeCount.Should().Be(1);
        genesisClientAdapterFake.CapturedThroughputProperties.Should().NotBeNull();

        // Assert - Container creation (without throughput)
        databaseFake.CreateContainerIfNotExistsAsyncInvokeCount.Should().Be(1);
        databaseFake.CreateContainerIfNotExistsAsyncWithThroughputInvokeCount.Should().Be(0);
        databaseFake.CapturedContainerProperties.Id.Should().Be(expectedContainerName);
        databaseFake.CapturedContainerProperties.PartitionKeyPath.Should().Be(expectedPartitionKeyPath);
        databaseFake.CapturedContainerProperties.DefaultTimeToLive.Should().Be(expectedTtl);

        // Assert - Invocation counts
        connectionConvenienceFake.DatabaseConfigInvokeCount.Should().Be(1);
        databaseConfigFake.ThroughputModeInvokeCount.Should().Be(1);
        databaseConfigFake.AutoscaleMaxInvokeCount.Should().Be(1);
        databaseConfigFake.ContainerConfigInvokeCount.Should().Be(1);
        containerConfigFake.TimeToLiveInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task LiveLongAndProsper_WithContainerLevelThroughput_CreatesDatabaseWithoutThroughputAndContainerWith()
    {
        // Arrange
        int expectedContainerAutoscaleMax = 1000;
        string expectedContainerName = "TestContainer";
        string expectedPartitionKeyPath = "/partitionKey";
        int? expectedTtl = 7200;

        CosmosContainerConfigFake containerConfigFake = new()
        {
            AutoscaleMaxResult = new CosmosAutoscaleMaxFake(expectedContainerAutoscaleMax),
            TimeToLiveResult = new CosmosContainerTimeToLiveFake(expectedTtl)
        };

        CosmosThroughputModeFake throughputModeFake = new() { IsDatabaseSharedResult = false };

        CosmosDatabaseConfigFake databaseConfigFake = new()
        {
            ThroughputModeResult = throughputModeFake,
            ContainerConfigResult = containerConfigFake
        };

        CosmosContainerDefinitionFake containerDefinitionFake = new()
        {
            ContainerNameResult = new CosmosContainerNameFake(expectedContainerName),
            PartitionKeyPathResult = new CosmosPartitionKeyPathFake(expectedPartitionKeyPath)
        };

        CosmosConnectionConvenienceFake connectionConvenienceFake = new()
        {
            DatabaseConfigResult = databaseConfigFake
        };

        DatabaseFake databaseFake = new();

        DatabaseResponseFake databaseResponseFake = new()
        {
            DatabaseResult = databaseFake
        };

        CosmosGenesisClientAdapterFake genesisClientAdapterFake = new()
        {
            CreateDatabaseIfNotExistsAsyncResult = databaseResponseFake
        };

        CosmosSasAuthGenesisDevice subject = new(containerDefinitionFake, connectionConvenienceFake);

        // Act
        await subject.LiveLongAndProsper(genesisClientAdapterFake).ConfigureAwait(false);

        // Assert - Database creation (without throughput)
        genesisClientAdapterFake.CreateDatabaseIfNotExistsAsyncInvokeCount.Should().Be(1);
        genesisClientAdapterFake.CapturedThroughputProperties.Should().BeNull();

        // Assert - Container creation (with throughput)
        databaseFake.CreateContainerIfNotExistsAsyncWithThroughputInvokeCount.Should().Be(1);
        databaseFake.CreateContainerIfNotExistsAsyncInvokeCount.Should().Be(0);
        databaseFake.CapturedContainerProperties.Id.Should().Be(expectedContainerName);
        databaseFake.CapturedContainerProperties.PartitionKeyPath.Should().Be(expectedPartitionKeyPath);
        databaseFake.CapturedContainerProperties.DefaultTimeToLive.Should().Be(expectedTtl);
        databaseFake.CapturedContainerThroughputProperties.Should().NotBeNull();

        // Assert - Invocation counts
        connectionConvenienceFake.DatabaseConfigInvokeCount.Should().Be(1);
        databaseConfigFake.ThroughputModeInvokeCount.Should().Be(1);
        databaseConfigFake.AutoscaleMaxInvokeCount.Should().Be(0);
        databaseConfigFake.ContainerConfigInvokeCount.Should().Be(1);
        containerConfigFake.AutoscaleMaxInvokeCount.Should().Be(1);
        containerConfigFake.TimeToLiveInvokeCount.Should().Be(1);
    }
}
