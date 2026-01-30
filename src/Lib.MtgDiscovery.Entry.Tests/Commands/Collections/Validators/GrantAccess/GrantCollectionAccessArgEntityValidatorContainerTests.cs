using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.GrantAccess;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.MtgDiscovery.Entry.Tests.Commands.Collections.Validators.GrantAccess;

[TestClass]
public sealed class GrantCollectionAccessArgEntityValidatorContainerTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithValidArgs_ReturnsValidResult()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901",
            Role = "editor"
        };
        GrantCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IGrantCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNullArgs_ReturnsInvalidResult()
    {
        // Arrange
        GrantCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IGrantCollectionAccessItrEntity>> actual =
            await subject.Validate(null!).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyCollectionId_ReturnsInvalidResult()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901",
            Role = "editor"
        };
        GrantCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IGrantCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidGuidCollectionId_ReturnsInvalidResult()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "not-a-valid-guid",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901",
            Role = "editor"
        };
        GrantCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IGrantCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyTargetUserId_ReturnsInvalidResult()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = "",
            Role = "editor"
        };
        GrantCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IGrantCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidGuidTargetUserId_ReturnsInvalidResult()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = "not-a-valid-guid",
            Role = "editor"
        };
        GrantCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IGrantCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithRoleEditor_ReturnsValidResult()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901",
            Role = "editor"
        };
        GrantCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IGrantCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithRoleViewer_ReturnsValidResult()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901",
            Role = "viewer"
        };
        GrantCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IGrantCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithRoleAdmin_ReturnsInvalidResult()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901",
            Role = "admin"
        };
        GrantCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IGrantCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidRole_ReturnsInvalidResult()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901",
            Role = "owner"
        };
        GrantCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IGrantCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyRole_ReturnsInvalidResult()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            TargetUserId = "b2c3d4e5-f6a7-8901-bcde-f12345678901",
            Role = ""
        };
        GrantCollectionAccessArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IGrantCollectionAccessItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    private sealed class GrantCollectionAccessArgEntityFake : IGrantCollectionAccessArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class IsNotNullGrantCollectionAccessArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNonNullArgs_ReturnsTrue()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new();
        IsNotNullGrantCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNullArgs_ReturnsFalse()
    {
        // Arrange
        IsNotNullGrantCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(null!).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        IsNotNullGrantCollectionAccessArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Grant collection access argument cannot be null");
    }

    private sealed class GrantCollectionAccessArgEntityFake : IGrantCollectionAccessArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidCollectionIdGrantCollectionAccessArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithValidGuid_ReturnsTrue()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new() { CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890" };
        HasValidCollectionIdGrantCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyCollectionId_ReturnsFalse()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new() { CollectionId = "" };
        HasValidCollectionIdGrantCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidGuid_ReturnsFalse()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new() { CollectionId = "not-a-guid" };
        HasValidCollectionIdGrantCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidCollectionIdGrantCollectionAccessArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection ID must be a valid GUID");
    }

    private sealed class GrantCollectionAccessArgEntityFake : IGrantCollectionAccessArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidTargetUserIdGrantCollectionAccessArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithValidGuid_ReturnsTrue()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new() { TargetUserId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890" };
        HasValidTargetUserIdGrantCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyTargetUserId_ReturnsFalse()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new() { TargetUserId = "" };
        HasValidTargetUserIdGrantCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidGuid_ReturnsFalse()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new() { TargetUserId = "not-a-guid" };
        HasValidTargetUserIdGrantCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidTargetUserIdGrantCollectionAccessArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Target user ID must be a valid GUID");
    }

    private sealed class GrantCollectionAccessArgEntityFake : IGrantCollectionAccessArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidRoleGrantCollectionAccessArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithRoleEditor_ReturnsTrue()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new() { Role = "editor" };
        HasValidRoleGrantCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithRoleViewer_ReturnsTrue()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new() { Role = "viewer" };
        HasValidRoleGrantCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidRole_ReturnsFalse()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new() { Role = "owner" };
        HasValidRoleGrantCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyRole_ReturnsFalse()
    {
        // Arrange
        GrantCollectionAccessArgEntityFake args = new() { Role = "" };
        HasValidRoleGrantCollectionAccessArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidRoleGrantCollectionAccessArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Role must be either 'editor' or 'viewer'");
    }

    private sealed class GrantCollectionAccessArgEntityFake : IGrantCollectionAccessArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string TargetUserId { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
    }
}
