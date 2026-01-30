using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Tests.Fakes;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
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
        CollectionOufEntityFake expectedOuf = new() { CollectionId = "col-123", Name = "Test" };
        OperationResponseFake<ICollectionOufEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedOuf
        };

        CollectionCommandAdapterFake commandFake = new()
        {
            CreateCollectionAsyncResult = expectedResponse
        };
        CollectionQueryAdapterFake queryFake = new();
        CollectionsAdapterService subject = new InstanceWrapper(commandFake, queryFake);

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
    public async Task GetDefaultCollectionAsync_DelegatesToQueryAdapter()
    {
        // Arrange
        CollectionOufEntityFake expectedOuf = new() { CollectionId = "col-default", IsDefault = true };
        OperationResponseFake<ICollectionOufEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedOuf
        };

        CollectionCommandAdapterFake commandFake = new();
        CollectionQueryAdapterFake queryFake = new()
        {
            GetDefaultCollectionAsyncResult = expectedResponse
        };
        CollectionsAdapterService subject = new InstanceWrapper(commandFake, queryFake);

        // Act
        IOperationResponse<ICollectionOufEntity> actual = await subject
            .GetDefaultCollectionAsync("user-123")
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        queryFake.GetDefaultCollectionAsyncInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetCollectionsByOwnerAsync_DelegatesToQueryAdapter()
    {
        // Arrange
        List<ICollectionOufEntity> expectedList = [new CollectionOufEntityFake { CollectionId = "col-1" }];
        OperationResponseFake<IEnumerable<ICollectionOufEntity>> expectedResponse = new()
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

        // Act
        IOperationResponse<IEnumerable<ICollectionOufEntity>> actual = await subject
            .GetCollectionsByOwnerAsync("user-123")
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
