using System.Net;
using System.Threading.Tasks;
using Lib.Cosmos.Adapters;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Cosmos.Operators;
using Lib.Cosmos.Tests.Fakes;
using Microsoft.Extensions.Logging;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Operators;

[TestClass]
public sealed class CosmosContainerReadOperatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task ReadAsync_ShouldCallContainerReadItemAsync()
    {
        // Arrange
        CosmosItemId itemId = new ProvidedCosmosItemId("itemId");
        PartitionKeyValue partitionKey = new ProvidedPartitionKeyValue("testPartition");
        ReadPointItem readItem = new() { Id = itemId, Partition = partitionKey };

        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = new TestItem { Id = itemId.ToString() },
            StatusCodeResult = HttpStatusCode.OK,
            RequestChargeResult = 3.5,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<TestItem> containerFake = new()
        {
            ReadItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerReadOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<TestItem> actual = await subject.ReadAsync<TestItem>(readItem).ConfigureAwait(false);

        // Assert
        containerFake.ReadItemAsyncInvokeCount.Should().Be(1);
        clientAdapterFake.GetContainerInvokeCount.Should().Be(1);
        actual.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public async Task ReadAsync_ShouldReturnItemOpResponseWithCorrectData()
    {
        // Arrange
        CosmosItemId itemId = new ProvidedCosmosItemId("itemId");
        PartitionKeyValue partitionKey = new ProvidedPartitionKeyValue("testPartition");
        ReadPointItem readItem = new() { Id = itemId, Partition = partitionKey };
        TestItem expectedItem = new() { Id = itemId.ToString(), Name = "Test Item" };

        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = expectedItem,
            StatusCodeResult = HttpStatusCode.OK,
            RequestChargeResult = 3.5,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<TestItem> containerFake = new()
        {
            ReadItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerReadOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<TestItem> actual = await subject.ReadAsync<TestItem>(readItem).ConfigureAwait(false);

        // Assert
        actual.Value.Should().Be(expectedItem);
        actual.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TestMethod, TestCategory("unit")]
    public async Task ReadAsync_WithNotFoundItem_ShouldReturnNotFoundResponse()
    {
        // Arrange
        CosmosItemId itemId = new ProvidedCosmosItemId("nonExistentId");
        PartitionKeyValue partitionKey = new ProvidedPartitionKeyValue("testPartition");
        ReadPointItem readItem = new() { Id = itemId, Partition = partitionKey };

        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = null,
            StatusCodeResult = HttpStatusCode.NotFound,
            RequestChargeResult = 1.0,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<TestItem> containerFake = new()
        {
            ReadItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerReadOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<TestItem> actual = await subject.ReadAsync<TestItem>(readItem).ConfigureAwait(false);

        // Assert
        actual.Value.Should().BeNull();
        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod, TestCategory("unit")]
    public async Task ReadAsync_ShouldLogReadInformation()
    {
        // Arrange
        CosmosItemId itemId = new ProvidedCosmosItemId("itemId");
        PartitionKeyValue partitionKey = new ProvidedPartitionKeyValue("testPartition");
        ReadPointItem readItem = new() { Id = itemId, Partition = partitionKey };
        double expectedRequestCharge = 3.5;

        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = new TestItem { Id = itemId.ToString() },
            StatusCodeResult = HttpStatusCode.OK,
            RequestChargeResult = expectedRequestCharge,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<TestItem> containerFake = new()
        {
            ReadItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerReadOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        _ = await subject.ReadAsync<TestItem>(readItem).ConfigureAwait(false);

        // Assert
        loggerFake.LogInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task ReadAsync_WithDifferentPartitionKeys_ShouldReadCorrectItem()
    {
        // Arrange
        CosmosItemId itemId = new ProvidedCosmosItemId("itemId");
        PartitionKeyValue partitionKey = new ProvidedPartitionKeyValue("partition123");
        ReadPointItem readItem = new() { Id = itemId, Partition = partitionKey };
        TestItem expectedItem = new() { Id = itemId.ToString(), Name = "Partition Test" };

        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = expectedItem,
            StatusCodeResult = HttpStatusCode.OK,
            RequestChargeResult = 3.5,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<TestItem> containerFake = new()
        {
            ReadItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerReadOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<TestItem> actual = await subject.ReadAsync<TestItem>(readItem).ConfigureAwait(false);

        // Assert
        actual.Value.Should().Be(expectedItem);
        containerFake.ReadItemAsyncInvokeCount.Should().Be(1);
    }

    private sealed class InstanceWrapper : TypeWrapper<CosmosContainerReadOperator>
    {
        public InstanceWrapper(ILogger logger, ICosmosClientAdapter clientAdapter) : base(logger, clientAdapter) { }
    }

    private sealed class TestItem
    {
        public string Id { get; init; }
        public string Name { get; init; }
    }
}
