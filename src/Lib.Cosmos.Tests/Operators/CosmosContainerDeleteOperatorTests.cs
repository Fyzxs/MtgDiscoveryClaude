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
public sealed class CosmosContainerDeleteOperatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task DeleteAsync_ShouldCallContainerDeleteItemAsync()
    {
        // Arrange
        CosmosItemId itemId = new ProvidedCosmosItemId("itemId");
        PartitionKeyValue partitionKey = new ProvidedPartitionKeyValue("testPartition");
        DeletePointItem deleteItem = new() { Id = itemId, Partition = partitionKey };

        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = new TestItem { Id = itemId },
            StatusCodeResult = HttpStatusCode.OK,
            RequestChargeResult = 5.5,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<TestItem> containerFake = new()
        {
            DeleteItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerDeleteOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<TestItem> result = await subject.DeleteAsync<TestItem>(deleteItem).ConfigureAwait(false);

        // Assert
        containerFake.DeleteItemAsyncInvokeCount.Should().Be(1);
        clientAdapterFake.GetContainerInvokeCount.Should().Be(1);
        result.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public async Task DeleteAsync_ShouldReturnItemOpResponseWithCorrectData()
    {
        // Arrange
        CosmosItemId itemId = new ProvidedCosmosItemId("itemId");
        PartitionKeyValue partitionKey = new ProvidedPartitionKeyValue("testPartition");
        DeletePointItem deleteItem = new() { Id = itemId, Partition = partitionKey };
        TestItem expectedItem = new() { Id = itemId };

        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = expectedItem,
            StatusCodeResult = HttpStatusCode.Accepted,
            RequestChargeResult = 5.5,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<TestItem> containerFake = new()
        {
            DeleteItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerDeleteOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<TestItem> result = await subject.DeleteAsync<TestItem>(deleteItem).ConfigureAwait(false);

        // Assert
        result.Value.Should().Be(expectedItem);
        result.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }

    private sealed class InstanceWrapper : TypeWrapper<CosmosContainerDeleteOperator>
    {
        public InstanceWrapper(ILogger logger, ICosmosClientAdapter clientAdapter) : base(logger, clientAdapter) { }
    }

    private sealed class TestItem
    {
        public string Id { get; init; }
    }
}
