using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Aggregator.Collections.Apis;
using Lib.Aggregator.Collections.Tests.Fakes;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Reflection;

namespace Lib.Aggregator.Collections.Tests.Apis;

[TestClass]
public sealed class CollectionsAggregatorServiceTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_ImplementsInterface()
    {
        // Arrange
        CollectionCommandAggregatorServiceFake commandFake = new();
        CollectionQueryAggregatorServiceFake queryFake = new();

        // Act
        CollectionsAggregatorService subject = new InstanceWrapper(commandFake, queryFake);

        // Assert
        subject.Should().BeAssignableTo<ICollectionsAggregatorService>();
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateCollectionAsync_DelegatesToCommandAggregator()
    {
        // Arrange
        CollectionOufEntityFake expectedOuf = new() { CollectionId = "col-123", Name = "Test" };
        OperationResponseFake<ICollectionOufEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedOuf
        };

        CollectionCommandAggregatorServiceFake commandFake = new()
        {
            CreateCollectionAsyncResult = expectedResponse
        };
        CollectionQueryAggregatorServiceFake queryFake = new();
        CollectionsAggregatorService subject = new InstanceWrapper(commandFake, queryFake);

        CollectionItrEntityFake itrEntity = new()
        {
            CollectionId = "col-123",
            Name = "Test",
            Type = "custom"
        };

        // Act
        IOperationResponse<ICollectionOufEntity> actual = await subject
            .CreateCollectionAsync(itrEntity)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        commandFake.CreateCollectionAsyncInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetDefaultCollectionAsync_DelegatesToQueryAggregator()
    {
        // Arrange
        CollectionOufEntityFake expectedOuf = new() { CollectionId = "col-default", IsDefault = true };
        OperationResponseFake<ICollectionOufEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedOuf
        };

        CollectionCommandAggregatorServiceFake commandFake = new();
        CollectionQueryAggregatorServiceFake queryFake = new()
        {
            GetDefaultCollectionAsyncResult = expectedResponse
        };
        CollectionsAggregatorService subject = new InstanceWrapper(commandFake, queryFake);

        // Act
        IOperationResponse<ICollectionOufEntity> actual = await subject
            .GetDefaultCollectionAsync("user-123")
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        queryFake.GetDefaultCollectionAsyncInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetCollectionsByOwnerAsync_DelegatesToQueryAggregator()
    {
        // Arrange
        List<ICollectionOufEntity> expectedList = [new CollectionOufEntityFake { CollectionId = "col-1" }];
        OperationResponseFake<IEnumerable<ICollectionOufEntity>> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedList
        };

        CollectionCommandAggregatorServiceFake commandFake = new();
        CollectionQueryAggregatorServiceFake queryFake = new()
        {
            GetCollectionsByOwnerAsyncResult = expectedResponse
        };
        CollectionsAggregatorService subject = new InstanceWrapper(commandFake, queryFake);

        // Act
        IOperationResponse<IEnumerable<ICollectionOufEntity>> actual = await subject
            .GetCollectionsByOwnerAsync("user-123")
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        queryFake.GetCollectionsByOwnerAsyncInvokeCount.Should().Be(1);
    }

    private sealed class InstanceWrapper : TypeWrapper<CollectionsAggregatorService>
    {
        public InstanceWrapper(
            ICollectionCommandAggregatorService commandAggregator,
            ICollectionQueryAggregatorService queryAggregator) : base(commandAggregator, queryAggregator) { }
    }
}
