using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Transfer;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.MtgDiscovery.Entry.Tests.Commands.Collections.Validators.Transfer;

[TestClass]
public sealed class TransferCollectionOwnershipArgEntityValidatorContainerTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithValidArgs_ReturnsValidResult()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901"
        };
        TransferCollectionOwnershipArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ITransferCollectionOwnershipItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNullArgs_ReturnsInvalidResult()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ITransferCollectionOwnershipItrEntity>> actual =
            await subject.Validate(null!).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyCollectionId_ReturnsInvalidResult()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityFake args = new()
        {
            CollectionId = "",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901"
        };
        TransferCollectionOwnershipArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ITransferCollectionOwnershipItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidGuidCollectionId_ReturnsInvalidResult()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityFake args = new()
        {
            CollectionId = "not-a-valid-guid",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901"
        };
        TransferCollectionOwnershipArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ITransferCollectionOwnershipItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyTargetUserId_ReturnsInvalidResult()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = ""
        };
        TransferCollectionOwnershipArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ITransferCollectionOwnershipItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidGuidTargetUserId_ReturnsInvalidResult()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = "not-a-valid-guid"
        };
        TransferCollectionOwnershipArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ITransferCollectionOwnershipItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    private sealed class TransferCollectionOwnershipArgEntityFake : ITransferCollectionOwnershipArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class IsNotNullTransferCollectionOwnershipArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNonNullArgs_ReturnsTrue()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityFake args = new();
        IsNotNullTransferCollectionOwnershipArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNullArgs_ReturnsFalse()
    {
        // Arrange
        IsNotNullTransferCollectionOwnershipArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(null!).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        IsNotNullTransferCollectionOwnershipArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Transfer collection ownership argument cannot be null");
    }

    private sealed class TransferCollectionOwnershipArgEntityFake : ITransferCollectionOwnershipArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidCollectionIdTransferCollectionOwnershipArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithValidGuid_ReturnsTrue()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityFake args = new() { CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890" };
        HasValidCollectionIdTransferCollectionOwnershipArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyCollectionId_ReturnsFalse()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityFake args = new() { CollectionId = "" };
        HasValidCollectionIdTransferCollectionOwnershipArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidGuid_ReturnsFalse()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityFake args = new() { CollectionId = "not-a-guid" };
        HasValidCollectionIdTransferCollectionOwnershipArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidCollectionIdTransferCollectionOwnershipArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection ID must be a valid GUID");
    }

    private sealed class TransferCollectionOwnershipArgEntityFake : ITransferCollectionOwnershipArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidTargetUserIdTransferCollectionOwnershipArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithValidGuid_ReturnsTrue()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityFake args = new() { TargetUserId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890" };
        HasValidTargetUserIdTransferCollectionOwnershipArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyTargetUserId_ReturnsFalse()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityFake args = new() { TargetUserId = "" };
        HasValidTargetUserIdTransferCollectionOwnershipArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidGuid_ReturnsFalse()
    {
        // Arrange
        TransferCollectionOwnershipArgEntityFake args = new() { TargetUserId = "not-a-guid" };
        HasValidTargetUserIdTransferCollectionOwnershipArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidTargetUserIdTransferCollectionOwnershipArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Target user ID must be a valid GUID");
    }

    private sealed class TransferCollectionOwnershipArgEntityFake : ITransferCollectionOwnershipArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
    }
}
