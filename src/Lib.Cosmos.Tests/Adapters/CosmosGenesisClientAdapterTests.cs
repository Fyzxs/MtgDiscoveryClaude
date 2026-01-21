using System.Threading.Tasks;
using Lib.Cosmos.Adapters;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Tests.Fakes;

namespace Lib.Cosmos.Tests.Adapters;

[TestClass]
public sealed class CosmosGenesisClientAdapterTests
{
    private sealed class TestCosmosGenesisClientAdapter : CosmosGenesisClientAdapter
    {
        public TestCosmosGenesisClientAdapter(CosmosClient cosmosClient) : base(cosmosClient) { }

        public static CosmosClientOptions OptionsAccess(ICosmosConnectionOptionsConfig config) => Options(config);
        protected override Task<DatabaseResponse> InternalCreateDatabaseIfNotExistsAsync(ICosmosContainerDefinition containerDefinition, CosmosClient cosmosClient) => throw new System.NotImplementedException();
    }

    [TestMethod, TestCategory("unit")]
    public void Options_WithConnectionModeOnly_ReturnsCorrectOptions()
    {
        // Arrange
        ConnectionMode expectedConnectionMode = ConnectionMode.Direct;
        CosmosConnectionOptionsConfigFake configFake = new()
        {
            ConnectionModeResult = new CosmosConnectionModeFake(expectedConnectionMode),
            PreferredRegionsResult = new CosmosPreferredRegionsFake()
        };

        // Act
        CosmosClientOptions result = TestCosmosGenesisClientAdapter.OptionsAccess(configFake);

        // Assert
        result.ConnectionMode.Should().Be(expectedConnectionMode);
        result.ApplicationPreferredRegions.Should().BeEmpty();
        configFake.ConnectionModeInvokeCount.Should().Be(1);
        configFake.PreferredRegionsInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public void Options_WithPreferredRegions_ReturnsCorrectOptions()
    {
        // Arrange
        ConnectionMode expectedConnectionMode = ConnectionMode.Gateway;
        string[] expectedRegions = ["East US", "West US", "Central US"];
        CosmosConnectionOptionsConfigFake configFake = new()
        {
            ConnectionModeResult = new CosmosConnectionModeFake(expectedConnectionMode),
            PreferredRegionsResult = new CosmosPreferredRegionsFake(expectedRegions)
        };

        // Act
        CosmosClientOptions result = TestCosmosGenesisClientAdapter.OptionsAccess(configFake);

        // Assert
        result.ConnectionMode.Should().Be(expectedConnectionMode);
        result.ApplicationPreferredRegions.Should().BeEquivalentTo(expectedRegions);
        configFake.ConnectionModeInvokeCount.Should().Be(1);
        configFake.PreferredRegionsInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetContainer_ReturnsContainer()
    {
        // Arrange
        string databaseName = "FakeTestDatabase";
        string containerName = "FakeTestContainer";
        Container expectedContainer = new ContainerFake<object>();

        using CosmosClientFake cosmosClientFake = new()
        {
            GetContainerResult = expectedContainer
        };
        CosmosGenesisClientAdapter subject = new TestCosmosGenesisClientAdapter(cosmosClientFake);

        CosmosContainerDefinitionFake containerDefFake = new()
        {
            DatabaseNameResult = new CosmosDatabaseNameFake(databaseName),
            ContainerNameResult = new CosmosContainerNameFake(containerName)
        };

        // Act
        Container result = await subject.GetContainer(containerDefFake).ConfigureAwait(false);

        // Assert
        result.Should().BeSameAs(expectedContainer);
        cosmosClientFake.GetContainerInvokeCount.Should().Be(1);
        containerDefFake.DatabaseNameInvokeCount.Should().Be(1);
        containerDefFake.ContainerNameInvokeCount.Should().Be(1);
    }
}
