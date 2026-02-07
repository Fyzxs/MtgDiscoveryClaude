using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Queries;
using Lib.Adapter.Collections.Queries.Mappers;
using Lib.Adapter.Collections.Tests.Fakes;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Adapter.Collections.Tests.Apis;

[TestClass]
public sealed class CollectionsAdapterServiceTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        LoggerFake logger = new();

        // Act
        CollectionsAdapterService subject = new(logger);

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

        CollectionsAdapterService subject = new InstanceWrapper(
            commandFake,
            new DefaultCollectionAdapterFake(),
            new CollectionsByOwnerAdapterFake(),
            new CollectionByIdAdapterFake(),
            new AccessibleCollectionsAdapterFake(),
            new OwnerIdXfrToUserIdXfrMapperFake());

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
    public async Task GetDefaultCollectionAsync_DelegatesToDefaultCollectionAdapter()
    {
        // Arrange
        CollectionExtEntity expectedExt = new() { CollectionId = "col-default", IsDefault = true };
        OperationResponseFake<CollectionExtEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedExt
        };

        DefaultCollectionAdapterFake defaultFake = new() { ExecuteResult = expectedResponse };

        CollectionsAdapterService subject = new InstanceWrapper(
            new CollectionCommandAdapterFake(),
            defaultFake,
            new CollectionsByOwnerAdapterFake(),
            new CollectionByIdAdapterFake(),
            new AccessibleCollectionsAdapterFake(),
            new OwnerIdXfrToUserIdXfrMapperFake());

        OwnerIdXfrEntityFake ownerIdXfr = new() { OwnerId = "user-123" };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .GetDefaultCollectionAsync(ownerIdXfr, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        defaultFake.ExecuteInvokeCount.Should().Be(1);
        defaultFake.LastExecuteInput!.UserId.Should().Be("user-123");
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetCollectionsByOwnerAsync_DelegatesToCollectionsByOwnerAdapter()
    {
        // Arrange
        List<CollectionExtEntity> expectedList = [new CollectionExtEntity { CollectionId = "col-1" }];
        OperationResponseFake<IEnumerable<CollectionExtEntity>> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedList
        };

        CollectionsByOwnerAdapterFake ownerFake = new() { ExecuteResult = expectedResponse };

        CollectionsAdapterService subject = new InstanceWrapper(
            new CollectionCommandAdapterFake(),
            new DefaultCollectionAdapterFake(),
            ownerFake,
            new CollectionByIdAdapterFake(),
            new AccessibleCollectionsAdapterFake(),
            new OwnerIdXfrToUserIdXfrMapperFake());

        OwnerIdXfrEntityFake ownerIdXfr = new() { OwnerId = "user-123" };

        // Act
        IOperationResponse<IEnumerable<CollectionExtEntity>> actual = await subject
            .GetCollectionsByOwnerAsync(ownerIdXfr, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        ownerFake.ExecuteInvokeCount.Should().Be(1);
        ownerFake.LastExecuteInput!.UserId.Should().Be("user-123");
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetCollectionByIdAsync_DelegatesToCollectionByIdAdapter()
    {
        // Arrange
        CollectionExtEntity expectedExt = new() { CollectionId = "col-123" };
        OperationResponseFake<CollectionExtEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedExt
        };

        CollectionByIdAdapterFake byIdFake = new() { ExecuteResult = expectedResponse };

        CollectionsAdapterService subject = new InstanceWrapper(
            new CollectionCommandAdapterFake(),
            new DefaultCollectionAdapterFake(),
            new CollectionsByOwnerAdapterFake(),
            byIdFake,
            new AccessibleCollectionsAdapterFake(),
            new OwnerIdXfrToUserIdXfrMapperFake());

        CollectionIdXfrEntityFake input = new()
        {
            CollectionId = "col-123",
            OwnerId = "owner-123"
        };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .GetCollectionByIdAsync(input, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        byIdFake.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task GetAccessibleCollectionsAsync_DelegatesToAccessibleCollectionsAdapter()
    {
        // Arrange
        List<CollectionExtEntity> expectedList =
        [
            new CollectionExtEntity { CollectionId = "col-owned", OwnerId = "user-123" },
            new CollectionExtEntity { CollectionId = "col-shared", OwnerId = "other-user" }
        ];
        OperationResponseFake<IEnumerable<CollectionExtEntity>> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedList
        };

        AccessibleCollectionsAdapterFake accessibleFake = new() { ExecuteResult = expectedResponse };

        CollectionsAdapterService subject = new InstanceWrapper(
            new CollectionCommandAdapterFake(),
            new DefaultCollectionAdapterFake(),
            new CollectionsByOwnerAdapterFake(),
            new CollectionByIdAdapterFake(),
            accessibleFake,
            new OwnerIdXfrToUserIdXfrMapperFake());

        UserIdXfrEntityFake input = new() { UserId = "user-123" };

        // Act
        IOperationResponse<IEnumerable<CollectionExtEntity>> actual = await subject
            .GetAccessibleCollectionsAsync(input, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        accessibleFake.ExecuteInvokeCount.Should().Be(1);
    }

    private sealed class InstanceWrapper : TypeWrapper<CollectionsAdapterService>
    {
        public InstanceWrapper(
            ICollectionCommandAdapter commandAdapter,
            IDefaultCollectionAdapter defaultAdapter,
            ICollectionsByOwnerAdapter ownerAdapter,
            ICollectionByIdAdapter byIdAdapter,
            IAccessibleCollectionsAdapter accessibleAdapter,
            IOwnerIdXfrToUserIdXfrMapper ownerIdXfrToUserIdXfrMapper)
            : base(commandAdapter, defaultAdapter, ownerAdapter, byIdAdapter, accessibleAdapter, ownerIdXfrToUserIdXfrMapper)
        { }
    }
}
