using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Lib.Cosmos.Adapters;
using Lib.Cosmos.Apis.Operators;
using Lib.Cosmos.Operators;
using Lib.Cosmos.Tests.Fakes;
using Microsoft.Azure.Cosmos;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Operators;

[TestClass]
public sealed class CosmosContainerQueryOperatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task QueryAsync_ShouldCallContainerGetItemQueryIterator()
    {
        // Arrange
        QueryDefinition queryDefinition = new("SELECT * FROM c");
        PartitionKey partitionKey = new("testPartition");
        Collection<TestItem> items = [new TestItem { Id = "1" }];

        FeedResponseFake<TestItem> feedResponseFake = new(items)
        {
            RequestChargeResult = 5.5,
            DiagnosticsResult = new CosmosDiagnosticsFake(),
            StatusCodeResult = HttpStatusCode.OK
        };

        FeedIteratorFake<TestItem> feedIteratorFake = new()
        {
            ReadNextAsyncResponse = feedResponseFake
        };

        ContainerFake<TestItem> containerFake = new()
        {
            GetItemQueryIteratorResponse = feedIteratorFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerQueryOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<IEnumerable<TestItem>> actual = await subject.QueryAsync<TestItem>(queryDefinition, partitionKey, TestContext.CancellationTokenSource.Token).ConfigureAwait(false);

        // Assert
        containerFake.GetItemQueryIteratorInvokeCount.Should().Be(1);
        clientAdapterFake.GetContainerInvokeCount.Should().Be(1);
        actual.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public async Task QueryAsync_ShouldReturnAllItemsInSingleCollection()
    {
        // Arrange
        QueryDefinition queryDefinition = new("SELECT * FROM c");
        PartitionKey partitionKey = new("testPartition");
        TestItem expectedItem1 = new() { Id = "1", Name = "First" };
        TestItem expectedItem2 = new() { Id = "2", Name = "Second" };
        Collection<TestItem> items = [expectedItem1, expectedItem2];

        FeedResponseFake<TestItem> feedResponseFake = new(items)
        {
            RequestChargeResult = 5.5,
            DiagnosticsResult = new CosmosDiagnosticsFake(),
            StatusCodeResult = HttpStatusCode.OK
        };

        FeedIteratorFake<TestItem> feedIteratorFake = new()
        {
            ReadNextAsyncResponse = feedResponseFake
        };

        ContainerFake<TestItem> containerFake = new()
        {
            GetItemQueryIteratorResponse = feedIteratorFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerQueryOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<IEnumerable<TestItem>> actual = await subject.QueryAsync<TestItem>(queryDefinition, partitionKey, TestContext.CancellationTokenSource.Token).ConfigureAwait(false);

        // Assert
        actual.Value.Should().HaveCount(2);
        actual.Value.Should().ContainInOrder(expectedItem1, expectedItem2);
        actual.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TestMethod, TestCategory("unit")]
    public async Task QueryAsync_WithEmptyResults_ShouldReturnEmptyCollection()
    {
        // Arrange
        QueryDefinition queryDefinition = new("SELECT * FROM c WHERE c.id = 'nonexistent'");
        PartitionKey partitionKey = new("testPartition");
        Collection<TestItem> items = [];

        FeedResponseFake<TestItem> feedResponseFake = new(items)
        {
            RequestChargeResult = 2.5,
            DiagnosticsResult = new CosmosDiagnosticsFake(),
            StatusCodeResult = HttpStatusCode.OK
        };

        FeedIteratorFake<TestItem> feedIteratorFake = new()
        {
            ReadNextAsyncResponse = feedResponseFake
        };

        ContainerFake<TestItem> containerFake = new()
        {
            GetItemQueryIteratorResponse = feedIteratorFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerQueryOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<IEnumerable<TestItem>> actual = await subject.QueryAsync<TestItem>(queryDefinition, partitionKey, TestContext.CancellationTokenSource.Token).ConfigureAwait(false);

        // Assert
        actual.Value.Should().BeEmpty();
        actual.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TestMethod, TestCategory("unit")]
    public async Task QueryAsync_ShouldLogQueryInformation()
    {
        // Arrange
        QueryDefinition queryDefinition = new("SELECT * FROM c");
        PartitionKey partitionKey = new("testPartition");
        double expectedRequestCharge = 7.5;
        Collection<TestItem> items = [new TestItem { Id = "1" }];

        FeedResponseFake<TestItem> feedResponseFake = new(items)
        {
            RequestChargeResult = expectedRequestCharge,
            DiagnosticsResult = new CosmosDiagnosticsFake(),
            StatusCodeResult = HttpStatusCode.OK
        };

        FeedIteratorFake<TestItem> feedIteratorFake = new()
        {
            ReadNextAsyncResponse = feedResponseFake
        };

        ContainerFake<TestItem> containerFake = new()
        {
            GetItemQueryIteratorResponse = feedIteratorFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerQueryOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        _ = await subject.QueryAsync<TestItem>(queryDefinition, partitionKey, TestContext.CancellationTokenSource.Token).ConfigureAwait(false);

        // Assert
        loggerFake.LogInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task QueryAsync_WithCancellationToken_ShouldPassTokenToIterator()
    {
        // Arrange
        QueryDefinition queryDefinition = new("SELECT * FROM c");
        PartitionKey partitionKey = new("testPartition");
        Collection<TestItem> items = [new TestItem { Id = "1" }];
        using CancellationTokenSource cts = new();

        FeedResponseFake<TestItem> feedResponseFake = new(items)
        {
            RequestChargeResult = 5.5,
            DiagnosticsResult = new CosmosDiagnosticsFake(),
            StatusCodeResult = HttpStatusCode.OK
        };

        FeedIteratorFake<TestItem> feedIteratorFake = new()
        {
            ReadNextAsyncResponse = feedResponseFake
        };

        ContainerFake<TestItem> containerFake = new()
        {
            GetItemQueryIteratorResponse = feedIteratorFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerQueryOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<IEnumerable<TestItem>> actual = await subject.QueryAsync<TestItem>(queryDefinition, partitionKey, cts.Token).ConfigureAwait(false);

        // Assert
        actual.Value.Should().HaveCount(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task QueryAsync_WithMultipleStatusCodes_ShouldReturnHighestStatusCode()
    {
        // Arrange
        QueryDefinition queryDefinition = new("SELECT * FROM c");
        PartitionKey partitionKey = new("testPartition");
        Collection<TestItem> items = [new TestItem { Id = "1" }];

        FeedResponseFake<TestItem> feedResponseFake = new(items)
        {
            RequestChargeResult = 5.5,
            DiagnosticsResult = new CosmosDiagnosticsFake(),
            StatusCodeResult = HttpStatusCode.Created
        };

        FeedIteratorFake<TestItem> feedIteratorFake = new()
        {
            ReadNextAsyncResponse = feedResponseFake
        };

        ContainerFake<TestItem> containerFake = new()
        {
            GetItemQueryIteratorResponse = feedIteratorFake
        };

        CosmosClientAdapterFake clientAdapterFake = new()
        {
            GetContainerResult = containerFake
        };

        LoggerFake loggerFake = new();
        CosmosContainerQueryOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        OpResponse<IEnumerable<TestItem>> actual = await subject.QueryAsync<TestItem>(queryDefinition, partitionKey, TestContext.CancellationTokenSource.Token).ConfigureAwait(false);

        // Assert
        actual.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    private sealed class InstanceWrapper : TypeWrapper<CosmosContainerQueryOperator>
    {
        public InstanceWrapper(ILogger logger, ICosmosClientAdapter clientAdapter) : base(logger, clientAdapter) { }
    }

    private sealed class TestItem
    {
        public string Id { get; init; }
        public string Name { get; init; }
    }

    public TestContext TestContext { get; set; }
}
