using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lib.Cosmos.Adapters;
using Lib.Cosmos.Apis.Operators;
using Lib.Cosmos.Operators;
using Lib.Cosmos.Tests.Fakes;
using Microsoft.Extensions.Logging;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Operators;

[TestClass]
public sealed class CosmosContainerUpsertOperatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task UpsertAsync_ShouldCallContainerUpsertItemAsync()
    {
        // Arrange
        TestItem itemToUpsert = new() { Id = "1", Name = "Test Item" };

        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = itemToUpsert,
            StatusCodeResult = HttpStatusCode.Created,
            RequestChargeResult = 8.5,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<TestItem> containerFake = new()
        {
            UpsertItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerUpsertOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<TestItem> actual = await subject.UpsertAsync(itemToUpsert).ConfigureAwait(false);

        // Assert
        containerFake.UpsertItemAsyncInvokeCount.Should().Be(1);
        clientAdapterFake.GetContainerInvokeCount.Should().Be(1);
        actual.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public async Task UpsertAsync_WithNewItem_ShouldReturnCreatedResponse()
    {
        // Arrange
        TestItem itemToUpsert = new() { Id = "1", Name = "New Item" };

        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = itemToUpsert,
            StatusCodeResult = HttpStatusCode.Created,
            RequestChargeResult = 8.5,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<TestItem> containerFake = new()
        {
            UpsertItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerUpsertOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<TestItem> actual = await subject.UpsertAsync(itemToUpsert).ConfigureAwait(false);

        // Assert
        actual.Value.Should().Be(itemToUpsert);
        actual.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [TestMethod, TestCategory("unit")]
    public async Task UpsertAsync_WithExistingItem_ShouldReturnOkResponse()
    {
        // Arrange
        TestItem itemToUpsert = new() { Id = "1", Name = "Updated Item" };

        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = itemToUpsert,
            StatusCodeResult = HttpStatusCode.OK,
            RequestChargeResult = 6.5,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<TestItem> containerFake = new()
        {
            UpsertItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerUpsertOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<TestItem> actual = await subject.UpsertAsync(itemToUpsert).ConfigureAwait(false);

        // Assert
        actual.Value.Should().Be(itemToUpsert);
        actual.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TestMethod, TestCategory("unit")]
    public async Task UpsertAsync_ShouldLogUpsertInformation()
    {
        // Arrange
        TestItem itemToUpsert = new() { Id = "1", Name = "Test Item" };
        const double ExpectedRequestCharge = 8.5;

        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = itemToUpsert,
            StatusCodeResult = HttpStatusCode.Created,
            RequestChargeResult = ExpectedRequestCharge,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<TestItem> containerFake = new()
        {
            UpsertItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerUpsertOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        _ = await subject.UpsertAsync(itemToUpsert).ConfigureAwait(false);

        // Assert
        loggerFake.LogInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task UpsertAsync_WithComplexObject_ShouldUpsertSuccessfully()
    {
        // Arrange
        ComplexTestItem itemToUpsert = new()
        {
            Id = "complex1",
            Name = "Complex Item",
            Tags = ["tag1", "tag2", "tag3"],
            Metadata = new Dictionary<string, string>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            }
        };

        ItemResponseFake<ComplexTestItem> itemResponseFake = new()
        {
            ResourceResult = itemToUpsert,
            StatusCodeResult = HttpStatusCode.Created,
            RequestChargeResult = 12.5,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<ComplexTestItem> containerFake = new()
        {
            UpsertItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerUpsertOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<ComplexTestItem> actual = await subject.UpsertAsync(itemToUpsert).ConfigureAwait(false);

        // Assert
        actual.Value.Should().Be(itemToUpsert);
        actual.Value.Tags.Should().HaveCount(3);
        actual.Value.Metadata.Should().HaveCount(2);
    }

    [TestMethod, TestCategory("unit")]
    public async Task UpsertAsync_ShouldReturnItemOpResponse()
    {
        // Arrange
        TestItem itemToUpsert = new() { Id = "1", Name = "Test Item" };

        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = itemToUpsert,
            StatusCodeResult = HttpStatusCode.Created,
            RequestChargeResult = 8.5,
            DiagnosticsResult = new CosmosDiagnosticsFake()
        };

        ContainerFake<TestItem> containerFake = new()
        {
            UpsertItemAsyncResponse = itemResponseFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerUpsertOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<TestItem> actual = await subject.UpsertAsync(itemToUpsert).ConfigureAwait(false);

        // Assert
        actual.Should().BeOfType<ItemOpResponse<TestItem>>();
    }

    private sealed class InstanceWrapper : TypeWrapper<CosmosContainerUpsertOperator>
    {
        public InstanceWrapper(ILogger logger, ICosmosClientAdapter clientAdapter) : base(logger, clientAdapter) { }
    }

    private sealed class TestItem
    {
        public string Id { get; init; }
        public string Name { get; init; }
    }

    private sealed class ComplexTestItem
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public List<string> Tags { get; init; }
        public Dictionary<string, string> Metadata { get; init; }
    }
}
