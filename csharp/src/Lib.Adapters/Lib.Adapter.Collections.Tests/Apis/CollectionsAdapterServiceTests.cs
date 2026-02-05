using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Tests.Fakes;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Reflection;

namespace Lib.Adapter.Collections.Tests.Apis;

[TestClass]
public sealed class CollectionsAdapterServiceTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_ImplementsInterface()
    {
        // Arrange
        CollectionCommandAdapterFake commandFake = new();
        CollectionQueryAdapterFake queryFake = new();

        // Act
        CollectionsAdapterService subject = new InstanceWrapper(commandFake, queryFake);

        // Assert
        subject.Should().BeAssignableTo<ICollectionsAdapterService>();
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateCollectionAsync_DelegatesToCommandAdapter()
    {
        // Arrange
        CollectionExtEntity expectedExt = new() { CollectionId = "col-123", Name = "Test" };
        OperationResponseFake<CollectionExtEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedExt
        };

        CollectionCommandAdapterFake commandFake = new()
        {
            CreateCollectionAsyncResult = expectedResponse
        };
        CollectionQueryAdapterFake queryFake = new();
        CollectionsAdapterService subject = new InstanceWrapper(commandFake, queryFake);

        CollectionXfrEntityFake xfrEntity = new()
        {
            CollectionId = "col-123",
            Name = "Test",
            Type = "custom"
        };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .CreateCollectionAsync(xfrEntity, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        commandFake.CreateCollectionAsyncInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetDefaultCollectionAsync_DelegatesToQueryAdapter()
    {
        // Arrange
        CollectionExtEntity expectedExt = new() { CollectionId = "col-default", IsDefault = true };
        OperationResponseFake<CollectionExtEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedExt
        };

        CollectionCommandAdapterFake commandFake = new();
        CollectionQueryAdapterFake queryFake = new()
        {
            GetDefaultCollectionAsyncResult = expectedResponse
        };
        CollectionsAdapterService subject = new InstanceWrapper(commandFake, queryFake);

        OwnerIdXfrEntityFake ownerIdXfr = new() { OwnerId = "user-123" };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .GetDefaultCollectionAsync(ownerIdXfr, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        queryFake.GetDefaultCollectionAsyncInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetCollectionsByOwnerAsync_DelegatesToQueryAdapter()
    {
        // Arrange
        List<CollectionExtEntity> expectedList = [new CollectionExtEntity { CollectionId = "col-1" }];
        OperationResponseFake<IEnumerable<CollectionExtEntity>> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedList
        };

        CollectionCommandAdapterFake commandFake = new();
        CollectionQueryAdapterFake queryFake = new()
        {
            GetCollectionsByOwnerAsyncResult = expectedResponse
        };
        CollectionsAdapterService subject = new InstanceWrapper(commandFake, queryFake);

        OwnerIdXfrEntityFake ownerIdXfr = new() { OwnerId = "user-123" };

        // Act
        IOperationResponse<IEnumerable<CollectionExtEntity>> actual = await subject
            .GetCollectionsByOwnerAsync(ownerIdXfr, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        queryFake.GetCollectionsByOwnerAsyncInvokeCount.Should().Be(1);
    }

    private sealed class InstanceWrapper : TypeWrapper<CollectionsAdapterService>
    {
        public InstanceWrapper(
            ICollectionCommandAdapter commandAdapter,
            ICollectionQueryAdapter queryAdapter) : base(commandAdapter, queryAdapter) { }
    }
}
