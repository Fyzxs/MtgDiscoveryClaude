using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Queries;
using Lib.Adapter.Collections.Tests.Fakes;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Adapter.Collections.Tests.Queries;

[TestClass]
public sealed class DefaultCollectionAdapterTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        LoggerFake logger = new();

        // Act
        DefaultCollectionAdapter subject = new(logger);

        // Assert
        subject.Should().BeAssignableTo<IDefaultCollectionAdapter>();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Execute_WithDefaultCollection_ReturnsDefaultCollection()
    {
        // Arrange
        CollectionExtEntity defaultCollection = new()
        {
            CollectionId = "col-default",
            OwnerId = "owner-123",
            Name = "Default",
            IsDefault = true
        };

        CollectionsByOwnerAdapterFake collectionsFake = new()
        {
            ExecuteResult = new SuccessOperationResponse<IEnumerable<CollectionExtEntity>>([
                new CollectionExtEntity { CollectionId = "col-1", IsDefault = false },
                defaultCollection,
                new CollectionExtEntity { CollectionId = "col-3", IsDefault = false }
            ])
        };

        DefaultCollectionAdapter subject = new InstanceWrapper(collectionsFake);
        UserIdXfrEntityFake input = new() { UserId = "owner-123" };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .Execute(input, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.CollectionId.Should().Be("col-default");
        actual.ResponseData.IsDefault.Should().BeTrue();
        collectionsFake.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task Execute_WithNoDefaultCollection_ReturnsFailure()
    {
        // Arrange
        CollectionsByOwnerAdapterFake collectionsFake = new()
        {
            ExecuteResult = new SuccessOperationResponse<IEnumerable<CollectionExtEntity>>([
                new CollectionExtEntity { CollectionId = "col-1", IsDefault = false },
                new CollectionExtEntity { CollectionId = "col-2", IsDefault = false }
            ])
        };

        DefaultCollectionAdapter subject = new InstanceWrapper(collectionsFake);
        UserIdXfrEntityFake input = new() { UserId = "owner-123" };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .Execute(input, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.IsFailure.Should().BeTrue();
        collectionsFake.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task Execute_WithEmptyCollections_ReturnsFailure()
    {
        // Arrange
        CollectionsByOwnerAdapterFake collectionsFake = new()
        {
            ExecuteResult = new SuccessOperationResponse<IEnumerable<CollectionExtEntity>>([])
        };

        DefaultCollectionAdapter subject = new InstanceWrapper(collectionsFake);
        UserIdXfrEntityFake input = new() { UserId = "owner-123" };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .Execute(input, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.IsFailure.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Execute_WhenCollectionsAdapterFails_ReturnsFailure()
    {
        // Arrange
        CollectionsByOwnerAdapterFake collectionsFake = new() { ShouldReturnFailure = true };

        DefaultCollectionAdapter subject = new InstanceWrapper(collectionsFake);
        UserIdXfrEntityFake input = new() { UserId = "owner-123" };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .Execute(input, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.IsFailure.Should().BeTrue();
        collectionsFake.ExecuteInvokeCount.Should().Be(1);
    }

    internal sealed class InstanceWrapper : TypeWrapper<DefaultCollectionAdapter>
    {
        public InstanceWrapper(ICollectionsByOwnerAdapter collectionsAdapter)
            : base(collectionsAdapter)
        { }
    }
}
