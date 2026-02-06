using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Commands;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Collections.Tests.Fakes;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Adapter.Collections.Tests.Commands;

[TestClass]
public sealed class CollectionCommandAdapterTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithLogger_CreatesInstance()
    {
        // Arrange
        LoggerFake logger = new();

        // Act
        CollectionCommandAdapter subject = new(logger);

        // Assert
        subject.Should().BeAssignableTo<ICollectionCommandAdapter>();
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateCollectionAsync_DelegatesToCreateAdapter()
    {
        // Arrange
        CollectionExtEntity expectedExt = new() { CollectionId = "col-123", Name = "Test" };
        OperationResponseFake<CollectionExtEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedExt
        };

        CreateCollectionAdapterFake createFake = new() { ExecuteResult = expectedResponse };
        RenameCollectionAdapterFake renameFake = new();
        UpdateCollectionVisibilityAdapterFake updateVisibilityFake = new();
        GrantCollectionAccessAdapterFake grantAccessFake = new();
        RevokeCollectionAccessAdapterFake revokeAccessFake = new();
        DeleteCollectionAdapterFake deleteFake = new();
        TransferCollectionOwnershipAdapterFake transferOwnershipFake = new();

        CollectionCommandAdapter subject = new InstanceWrapper(
            createFake, renameFake, updateVisibilityFake,
            grantAccessFake, revokeAccessFake, deleteFake, transferOwnershipFake);

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
        createFake.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateCollectionAsync_WhenAdapterFails_ReturnsFailure()
    {
        // Arrange
        CreateCollectionAdapterFake createFake = new() { ShouldReturnFailure = true };
        RenameCollectionAdapterFake renameFake = new();
        UpdateCollectionVisibilityAdapterFake updateVisibilityFake = new();
        GrantCollectionAccessAdapterFake grantAccessFake = new();
        RevokeCollectionAccessAdapterFake revokeAccessFake = new();
        DeleteCollectionAdapterFake deleteFake = new();
        TransferCollectionOwnershipAdapterFake transferOwnershipFake = new();

        CollectionCommandAdapter subject = new InstanceWrapper(
            createFake, renameFake, updateVisibilityFake,
            grantAccessFake, revokeAccessFake, deleteFake, transferOwnershipFake);

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
        actual.IsFailure.Should().BeTrue();
        actual.OuterException.Should().BeOfType<CollectionAdapterException>();
        createFake.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task RenameCollectionAsync_DelegatesToRenameAdapter()
    {
        // Arrange
        CollectionExtEntity expectedExt = new() { CollectionId = "col-123", Name = "Renamed" };
        OperationResponseFake<CollectionExtEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedExt
        };

        CreateCollectionAdapterFake createFake = new();
        RenameCollectionAdapterFake renameFake = new() { ExecuteResult = expectedResponse };
        UpdateCollectionVisibilityAdapterFake updateVisibilityFake = new();
        GrantCollectionAccessAdapterFake grantAccessFake = new();
        RevokeCollectionAccessAdapterFake revokeAccessFake = new();
        DeleteCollectionAdapterFake deleteFake = new();
        TransferCollectionOwnershipAdapterFake transferOwnershipFake = new();

        CollectionCommandAdapter subject = new InstanceWrapper(
            createFake, renameFake, updateVisibilityFake,
            grantAccessFake, revokeAccessFake, deleteFake, transferOwnershipFake);

        RenameCollectionXfrEntityFake xfrEntity = new()
        {
            CollectionId = "col-123",
            OwnerId = "owner-123",
            Name = "Renamed"
        };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .RenameCollectionAsync(xfrEntity, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        renameFake.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task UpdateCollectionVisibilityAsync_DelegatesToUpdateVisibilityAdapter()
    {
        // Arrange
        CollectionExtEntity expectedExt = new() { CollectionId = "col-123", Visibility = "public" };
        OperationResponseFake<CollectionExtEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedExt
        };

        CreateCollectionAdapterFake createFake = new();
        RenameCollectionAdapterFake renameFake = new();
        UpdateCollectionVisibilityAdapterFake updateVisibilityFake = new() { ExecuteResult = expectedResponse };
        GrantCollectionAccessAdapterFake grantAccessFake = new();
        RevokeCollectionAccessAdapterFake revokeAccessFake = new();
        DeleteCollectionAdapterFake deleteFake = new();
        TransferCollectionOwnershipAdapterFake transferOwnershipFake = new();

        CollectionCommandAdapter subject = new InstanceWrapper(
            createFake, renameFake, updateVisibilityFake,
            grantAccessFake, revokeAccessFake, deleteFake, transferOwnershipFake);

        UpdateCollectionVisibilityXfrEntityFake xfrEntity = new()
        {
            CollectionId = "col-123",
            OwnerId = "owner-123",
            Visibility = "public"
        };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .UpdateCollectionVisibilityAsync(xfrEntity, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        updateVisibilityFake.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task GrantCollectionAccessAsync_DelegatesToGrantAccessAdapter()
    {
        // Arrange
        CollectionExtEntity expectedExt = new()
        {
            CollectionId = "col-123",
            AuthorizedUsers = [new AuthorizedUserExtEntity { UserId = "target-user", Role = "viewer" }]
        };
        OperationResponseFake<CollectionExtEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedExt
        };

        CreateCollectionAdapterFake createFake = new();
        RenameCollectionAdapterFake renameFake = new();
        UpdateCollectionVisibilityAdapterFake updateVisibilityFake = new();
        GrantCollectionAccessAdapterFake grantAccessFake = new() { ExecuteResult = expectedResponse };
        RevokeCollectionAccessAdapterFake revokeAccessFake = new();
        DeleteCollectionAdapterFake deleteFake = new();
        TransferCollectionOwnershipAdapterFake transferOwnershipFake = new();

        CollectionCommandAdapter subject = new InstanceWrapper(
            createFake, renameFake, updateVisibilityFake,
            grantAccessFake, revokeAccessFake, deleteFake, transferOwnershipFake);

        GrantCollectionAccessXfrEntityFake xfrEntity = new()
        {
            CollectionId = "col-123",
            GrantorUserId = "owner-123",
            TargetUserId = "target-user",
            Role = "viewer"
        };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .GrantCollectionAccessAsync(xfrEntity, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        grantAccessFake.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task RevokeCollectionAccessAsync_DelegatesToRevokeAccessAdapter()
    {
        // Arrange
        CollectionExtEntity expectedExt = new() { CollectionId = "col-123", AuthorizedUsers = [] };
        OperationResponseFake<CollectionExtEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedExt
        };

        CreateCollectionAdapterFake createFake = new();
        RenameCollectionAdapterFake renameFake = new();
        UpdateCollectionVisibilityAdapterFake updateVisibilityFake = new();
        GrantCollectionAccessAdapterFake grantAccessFake = new();
        RevokeCollectionAccessAdapterFake revokeAccessFake = new() { ExecuteResult = expectedResponse };
        DeleteCollectionAdapterFake deleteFake = new();
        TransferCollectionOwnershipAdapterFake transferOwnershipFake = new();

        CollectionCommandAdapter subject = new InstanceWrapper(
            createFake, renameFake, updateVisibilityFake,
            grantAccessFake, revokeAccessFake, deleteFake, transferOwnershipFake);

        RevokeCollectionAccessXfrEntityFake xfrEntity = new()
        {
            CollectionId = "col-123",
            RevokerUserId = "owner-123",
            TargetUserId = "target-user"
        };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .RevokeCollectionAccessAsync(xfrEntity, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        revokeAccessFake.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task DeleteCollectionAsync_DelegatesToDeleteAdapter()
    {
        // Arrange
        CollectionExtEntity expectedExt = new() { CollectionId = "col-123", Name = "Deleted" };
        OperationResponseFake<CollectionExtEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedExt
        };

        CreateCollectionAdapterFake createFake = new();
        RenameCollectionAdapterFake renameFake = new();
        UpdateCollectionVisibilityAdapterFake updateVisibilityFake = new();
        GrantCollectionAccessAdapterFake grantAccessFake = new();
        RevokeCollectionAccessAdapterFake revokeAccessFake = new();
        DeleteCollectionAdapterFake deleteFake = new() { ExecuteResult = expectedResponse };
        TransferCollectionOwnershipAdapterFake transferOwnershipFake = new();

        CollectionCommandAdapter subject = new InstanceWrapper(
            createFake, renameFake, updateVisibilityFake,
            grantAccessFake, revokeAccessFake, deleteFake, transferOwnershipFake);

        DeleteCollectionXfrEntityFake xfrEntity = new()
        {
            CollectionId = "col-123",
            OwnerId = "owner-123"
        };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .DeleteCollectionAsync(xfrEntity, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        deleteFake.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task TransferCollectionOwnershipAsync_DelegatesToTransferOwnershipAdapter()
    {
        // Arrange
        CollectionExtEntity expectedExt = new() { CollectionId = "col-123", OwnerId = "new-owner" };
        OperationResponseFake<CollectionExtEntity> expectedResponse = new()
        {
            IsSuccess = true,
            ResponseData = expectedExt
        };

        CreateCollectionAdapterFake createFake = new();
        RenameCollectionAdapterFake renameFake = new();
        UpdateCollectionVisibilityAdapterFake updateVisibilityFake = new();
        GrantCollectionAccessAdapterFake grantAccessFake = new();
        RevokeCollectionAccessAdapterFake revokeAccessFake = new();
        DeleteCollectionAdapterFake deleteFake = new();
        TransferCollectionOwnershipAdapterFake transferOwnershipFake = new() { ExecuteResult = expectedResponse };

        CollectionCommandAdapter subject = new InstanceWrapper(
            createFake, renameFake, updateVisibilityFake,
            grantAccessFake, revokeAccessFake, deleteFake, transferOwnershipFake);

        TransferCollectionOwnershipXfrEntityFake xfrEntity = new()
        {
            CollectionId = "col-123",
            CurrentOwnerId = "owner-123",
            TargetUserId = "new-owner"
        };

        // Act
        IOperationResponse<CollectionExtEntity> actual = await subject
            .TransferCollectionOwnershipAsync(xfrEntity, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        actual.Should().Be(expectedResponse);
        transferOwnershipFake.ExecuteInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task CreateCollectionAsync_OnlyInvokesCreateAdapter()
    {
        // Arrange
        CreateCollectionAdapterFake createFake = new();
        RenameCollectionAdapterFake renameFake = new();
        UpdateCollectionVisibilityAdapterFake updateVisibilityFake = new();
        GrantCollectionAccessAdapterFake grantAccessFake = new();
        RevokeCollectionAccessAdapterFake revokeAccessFake = new();
        DeleteCollectionAdapterFake deleteFake = new();
        TransferCollectionOwnershipAdapterFake transferOwnershipFake = new();

        CollectionCommandAdapter subject = new InstanceWrapper(
            createFake, renameFake, updateVisibilityFake,
            grantAccessFake, revokeAccessFake, deleteFake, transferOwnershipFake);

        CollectionXfrEntityFake xfrEntity = new() { CollectionId = "col-123", Name = "Test" };

        // Act
        _ = await subject
            .CreateCollectionAsync(xfrEntity, CancellationToken.None)
            .ConfigureAwait(false);

        // Assert
        createFake.ExecuteInvokeCount.Should().Be(1);
        renameFake.ExecuteInvokeCount.Should().Be(0);
        updateVisibilityFake.ExecuteInvokeCount.Should().Be(0);
        grantAccessFake.ExecuteInvokeCount.Should().Be(0);
        revokeAccessFake.ExecuteInvokeCount.Should().Be(0);
        deleteFake.ExecuteInvokeCount.Should().Be(0);
        transferOwnershipFake.ExecuteInvokeCount.Should().Be(0);
    }

    internal sealed class InstanceWrapper : TypeWrapper<CollectionCommandAdapter>
    {
        public InstanceWrapper(
            ICreateCollectionAdapter createAdapter,
            IRenameCollectionAdapter renameAdapter,
            IUpdateCollectionVisibilityAdapter updateVisibilityAdapter,
            IGrantCollectionAccessAdapter grantAccessAdapter,
            IRevokeCollectionAccessAdapter revokeAccessAdapter,
            IDeleteCollectionAdapter deleteAdapter,
            ITransferCollectionOwnershipAdapter transferOwnershipAdapter)
            : base(createAdapter, renameAdapter, updateVisibilityAdapter,
                   grantAccessAdapter, revokeAccessAdapter, deleteAdapter, transferOwnershipAdapter)
        { }
    }
}
