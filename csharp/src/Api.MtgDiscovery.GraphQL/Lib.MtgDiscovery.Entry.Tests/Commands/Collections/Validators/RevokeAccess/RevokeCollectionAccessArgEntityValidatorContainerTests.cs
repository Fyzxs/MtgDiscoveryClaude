using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.RevokeAccess;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.MtgDiscovery.Entry.Tests.Commands.Collections.Validators.RevokeAccess;

[TestClass]
public sealed class RevokeCollectionAccessArgEntityValidatorContainerTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithValidArgs_ReturnsValidResult()
    {
        // Arrange
        RevokeCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901"
        };
        RevokeCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRevokeCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNullArgs_ReturnsInvalidResult()
    {
        // Arrange
        RevokeCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRevokeCollectionAccessItrEntity>> actual =
            await subject.Validate(null!).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyCollectionId_ReturnsInvalidResult()
    {
        // Arrange
        RevokeCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901"
        };
        RevokeCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRevokeCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidGuidCollectionId_ReturnsInvalidResult()
    {
        // Arrange
        RevokeCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "not-a-valid-guid",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901"
        };
        RevokeCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRevokeCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyTargetUserId_ReturnsInvalidResult()
    {
        // Arrange
        RevokeCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = ""
        };
        RevokeCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRevokeCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidGuidTargetUserId_ReturnsInvalidResult()
    {
        // Arrange
        RevokeCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = "not-a-valid-guid"
        };
        RevokeCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRevokeCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    private sealed class RevokeCollectionAccessArgEntityFake : IRevokeCollectionAccessArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class IsNotNullRevokeCollectionAccessArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNonNullArgs_ReturnsTrue()
    {
        // Arrange
        RevokeCollectionAccessArgEntityFake args = new();
        IsNotNullRevokeCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNullArgs_ReturnsFalse()
    {
        // Arrange
        IsNotNullRevokeCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(null!).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        IsNotNullRevokeCollectionAccessArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Revoke collection access argument cannot be null");
    }

    private sealed class RevokeCollectionAccessArgEntityFake : IRevokeCollectionAccessArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidCollectionIdRevokeCollectionAccessArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithValidGuid_ReturnsTrue()
    {
        // Arrange
        RevokeCollectionAccessArgEntityFake args = new() { CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890" };
        HasValidCollectionIdRevokeCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyCollectionId_ReturnsFalse()
    {
        // Arrange
        RevokeCollectionAccessArgEntityFake args = new() { CollectionId = "" };
        HasValidCollectionIdRevokeCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidGuid_ReturnsFalse()
    {
        // Arrange
        RevokeCollectionAccessArgEntityFake args = new() { CollectionId = "not-a-guid" };
        HasValidCollectionIdRevokeCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidCollectionIdRevokeCollectionAccessArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection ID must be a valid GUID");
    }

    private sealed class RevokeCollectionAccessArgEntityFake : IRevokeCollectionAccessArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidTargetUserIdRevokeCollectionAccessArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithValidGuid_ReturnsTrue()
    {
        // Arrange
        RevokeCollectionAccessArgEntityFake args = new() { TargetUserId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890" };
        HasValidTargetUserIdRevokeCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyTargetUserId_ReturnsFalse()
    {
        // Arrange
        RevokeCollectionAccessArgEntityFake args = new() { TargetUserId = "" };
        HasValidTargetUserIdRevokeCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidGuid_ReturnsFalse()
    {
        // Arrange
        RevokeCollectionAccessArgEntityFake args = new() { TargetUserId = "not-a-guid" };
        HasValidTargetUserIdRevokeCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidTargetUserIdRevokeCollectionAccessArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Target user ID must be a valid GUID");
    }

    private sealed class RevokeCollectionAccessArgEntityFake : IRevokeCollectionAccessArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
    }
}
