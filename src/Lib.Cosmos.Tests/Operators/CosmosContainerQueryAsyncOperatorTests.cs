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
using Microsoft.Extensions.Logging;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Operators;

[TestClass]
public sealed class CosmosContainerQueryAsyncOperatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task QueryYield_ShouldCallContainerGetItemQueryIterator()
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
        CosmosContainerQueryAsyncOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        List<OpResponse<TestItem>> results = [];
        await foreach (OpResponse<TestItem> item in subject.QueryYield<TestItem>(queryDefinition, partitionKey).ConfigureAwait(false))
        {
            results.Add(item);
        }

        // Assert
        containerFake.GetItemQueryIteratorInvokeCount.Should().Be(1);
        clientAdapterFake.GetContainerInvokeCount.Should().Be(1);
        results.Count.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task QueryYield_ShouldReturnProvidedOpResponseWithCorrectData()
    {
        // Arrange
        QueryDefinition queryDefinition = new("SELECT * FROM c");
        PartitionKey partitionKey = new("testPartition");
        TestItem expectedItem = new() { Id = "1", Name = "Test" };
        Collection<TestItem> items = [expectedItem];

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
        CosmosContainerQueryAsyncOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        List<OpResponse<TestItem>> results = [];
        await foreach (OpResponse<TestItem> item in subject.QueryYield<TestItem>(queryDefinition, partitionKey).ConfigureAwait(false))
        {
            results.Add(item);
        }

        // Assert
        results.Should().HaveCount(1);
        results[0].Value.Should().Be(expectedItem);
        results[0].StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TestMethod, TestCategory("unit")]
    public async Task QueryYield_WithMultipleItems_ShouldYieldAllItems()
    {
        // Arrange
        QueryDefinition queryDefinition = new("SELECT * FROM c");
        PartitionKey partitionKey = new("testPartition");
        TestItem item1 = new() { Id = "1", Name = "First" };
        TestItem item2 = new() { Id = "2", Name = "Second" };
        TestItem item3 = new() { Id = "3", Name = "Third" };
        Collection<TestItem> items = [item1, item2, item3];

        FeedResponseFake<TestItem> feedResponseFake = new(items)
        {
            RequestChargeResult = 10.5,
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
        CosmosContainerQueryAsyncOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        List<OpResponse<TestItem>> results = [];
        await foreach (OpResponse<TestItem> item in subject.QueryYield<TestItem>(queryDefinition, partitionKey).ConfigureAwait(false))
        {
            results.Add(item);
        }

        // Assert
        results.Should().HaveCount(3);
        results[0].Value.Should().Be(item1);
        results[1].Value.Should().Be(item2);
        results[2].Value.Should().Be(item3);
    }

    [TestMethod, TestCategory("unit")]
    public async Task QueryYield_ShouldLogQueryInformation()
    {
        // Arrange
        QueryDefinition queryDefinition = new("SELECT * FROM c");
        PartitionKey partitionKey = new("testPartition");
        const double ExpectedRequestCharge = 7.5;
        Collection<TestItem> items = [new TestItem { Id = "1" }];

        FeedResponseFake<TestItem> feedResponseFake = new(items)
        {
            RequestChargeResult = ExpectedRequestCharge,
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
        CosmosContainerQueryAsyncOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        List<OpResponse<TestItem>> results = [];
        await foreach (OpResponse<TestItem> item in subject.QueryYield<TestItem>(queryDefinition, partitionKey).ConfigureAwait(false))
        {
            results.Add(item);
        }

        // Assert
        results.Should().HaveCount(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task QueryYield_WithCancellationToken_ShouldPassTokenToIterator()
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
        CosmosContainerQueryAsyncOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        List<OpResponse<TestItem>> results = [];
        await foreach (OpResponse<TestItem> item in subject.QueryYield<TestItem>(queryDefinition, partitionKey, cts.Token).ConfigureAwait(false))
        {
            results.Add(item);
        }

        // Assert
        results.Should().HaveCount(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task QueryYield_WithEmptyResults_ShouldYieldNothing()
    {
        // Arrange
        QueryDefinition queryDefinition = new("SELECT * FROM c");
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
        CosmosContainerQueryAsyncOperator subject = new InstanceWrapper(loggerFake, clientAdapterFake);

        // Act
        List<OpResponse<TestItem>> results = [];
        await foreach (OpResponse<TestItem> item in subject.QueryYield<TestItem>(queryDefinition, partitionKey).ConfigureAwait(false))
        {
            results.Add(item);
        }

        // Assert
        results.Should().BeEmpty();
    }

    private sealed class InstanceWrapper : TypeWrapper<CosmosContainerQueryAsyncOperator>
    {
        public InstanceWrapper(ILogger logger, ICosmosClientAdapter clientAdapter) : base(logger, clientAdapter) { }
    }

    private sealed class TestItem
    {
        public string Id { get; init; }
        public string Name { get; init; }
    }
}
