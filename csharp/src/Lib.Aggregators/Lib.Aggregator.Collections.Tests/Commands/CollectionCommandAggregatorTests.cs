using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Aggregator.Collections.Commands;
using Lib.Aggregator.Collections.Commands.Mappers;
using Lib.Aggregator.Collections.Mappers;
using Lib.Aggregator.Collections.Tests.Fakes;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Reflection;

namespace Lib.Aggregator.Collections.Tests.Commands;

[TestClass]
public sealed class CollectionCommandAggregatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task CreateCollectionAsync_WithSuccess_ReturnsDelegatedResponse()
    {
        // Arrange
        CollectionExtEntity expectedExt = new()
        {
            CollectionId = "col-123",
            OwnerId = "user-123",
            Name = "Test Collection",
            Type = "custom"
        };

        CollectionsAdapterServiceFake adapterFake = new()
        {
            CreateCollectionAsyncResult = new OperationResponseFake<CollectionExtEntity>
            {
                IsSuccess = true,
                ResponseData = expectedExt
            }
        };

        CollectionCommandAggregator subject = new InstanceWrapper(
            adapterFake,
            new CollectionExtToOufMapper(),
            new CollectionItrToXfrMapper(),
            new RenameCollectionItrToXfrMapper(),
            new UpdateCollectionVisibilityItrToXfrMapper(),
            new GrantCollectionAccessItrToXfrMapper(),
            new RevokeCollectionAccessItrToXfrMapper(),
            new DeleteCollectionItrToXfrMapper(),
            new TransferCollectionOwnershipItrToXfrMapper());

        CollectionItrEntityFake itrEntity = new()
        {
            CollectionId = "col-123",
            OwnerId = "user-123",
            Name = "Test Collection",
            Type = "custom"
        };

        // Act
        IOperationResponse<ICollectionOufEntity> actual = await subject
            .CreateCollectionAsync(itrEntity, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.ResponseData.CollectionId.Should().Be("col-123");
        adapterFake.CreateCollectionAsyncInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateCollectionAsync_WhenAdapterFails_ReturnsFailure()
    {
        // Arrange
        CollectionsAdapterServiceFake adapterFake = new()
        {
            CreateCollectionAsyncResult = new OperationResponseFake<CollectionExtEntity>
            {
                IsSuccess = false,
                OuterException = new Lib.Shared.Invocation.Exceptions.BadRequestOperationException("Adapter error")
            }
        };

        CollectionCommandAggregator subject = new InstanceWrapper(
            adapterFake,
            new CollectionExtToOufMapper(),
            new CollectionItrToXfrMapper(),
            new RenameCollectionItrToXfrMapper(),
            new UpdateCollectionVisibilityItrToXfrMapper(),
            new GrantCollectionAccessItrToXfrMapper(),
            new RevokeCollectionAccessItrToXfrMapper(),
            new DeleteCollectionItrToXfrMapper(),
            new TransferCollectionOwnershipItrToXfrMapper());

        CollectionItrEntityFake itrEntity = new()
        {
            CollectionId = "col-123",
            Name = "Test",
            Type = "custom"
        };

        // Act
        IOperationResponse<ICollectionOufEntity> actual = await subject
            .CreateCollectionAsync(itrEntity, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.IsFailure.Should().BeTrue();
        adapterFake.CreateCollectionAsyncInvokeCount.Should().Be(1);
    }

    private sealed class InstanceWrapper : TypeWrapper<CollectionCommandAggregator>
    {
        public InstanceWrapper(
            ICollectionsAdapterService adapterService,
            ICollectionExtToOufMapper extToOufMapper,
            ICollectionItrToXfrMapper collectionItrToXfrMapper,
            IRenameCollectionItrToXfrMapper renameCollectionItrToXfrMapper,
            IUpdateCollectionVisibilityItrToXfrMapper updateVisibilityItrToXfrMapper,
            IGrantCollectionAccessItrToXfrMapper grantAccessItrToXfrMapper,
            IRevokeCollectionAccessItrToXfrMapper revokeAccessItrToXfrMapper,
            IDeleteCollectionItrToXfrMapper deleteCollectionItrToXfrMapper,
            ITransferCollectionOwnershipItrToXfrMapper transferOwnershipItrToXfrMapper)
            : base(
                adapterService,
                extToOufMapper,
                collectionItrToXfrMapper,
                renameCollectionItrToXfrMapper,
                updateVisibilityItrToXfrMapper,
                grantAccessItrToXfrMapper,
                revokeAccessItrToXfrMapper,
                deleteCollectionItrToXfrMapper,
                transferOwnershipItrToXfrMapper)
        {
        }
    }
}
