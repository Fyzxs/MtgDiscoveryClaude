using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Rename;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.MtgDiscovery.Entry.Tests.Commands.Collections.Validators.Rename;

[TestClass]
public sealed class RenameCollectionArgEntityValidatorContainerTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithValidArgs_ReturnsValidResult()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            Name = "My Renamed Collection"
        };
        RenameCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRenameCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNullArgs_ReturnsInvalidResult()
    {
        // Arrange
        RenameCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRenameCollectionItrEntity>> actual =
            await subject.Validate(null!).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyCollectionId_ReturnsInvalidResult()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new()
        {
            CollectionId = "",
            Name = "My Collection"
        };
        RenameCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRenameCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidGuidCollectionId_ReturnsInvalidResult()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new()
        {
            CollectionId = "not-a-valid-guid",
            Name = "My Collection"
        };
        RenameCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRenameCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyName_ReturnsInvalidResult()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            Name = ""
        };
        RenameCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRenameCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithWhitespaceName_ReturnsInvalidResult()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            Name = "   "
        };
        RenameCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRenameCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNameTooLong_ReturnsInvalidResult()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            Name = new string('A', 101)
        };
        RenameCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRenameCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNameExactly100Chars_ReturnsValidResult()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            Name = new string('A', 100)
        };
        RenameCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRenameCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithReservedNameDefault_ReturnsInvalidResult()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            Name = "default"
        };
        RenameCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRenameCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithReservedNameDefaultUpperCase_ReturnsInvalidResult()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            Name = "DEFAULT"
        };
        RenameCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IRenameCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    private sealed class RenameCollectionArgEntityFake : IRenameCollectionArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class IsNotNullRenameCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNonNullArgs_ReturnsTrue()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new();
        IsNotNullRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNullArgs_ReturnsFalse()
    {
        // Arrange
        IsNotNullRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(null!).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        IsNotNullRenameCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Rename collection argument cannot be null");
    }

    private sealed class RenameCollectionArgEntityFake : IRenameCollectionArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidCollectionIdRenameCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithValidGuid_ReturnsTrue()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890" };
        HasValidCollectionIdRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyCollectionId_ReturnsFalse()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { CollectionId = "" };
        HasValidCollectionIdRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidGuid_ReturnsFalse()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { CollectionId = "not-a-guid" };
        HasValidCollectionIdRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidCollectionIdRenameCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection ID must be a valid GUID");
    }

    private sealed class RenameCollectionArgEntityFake : IRenameCollectionArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidNameRenameCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithValidName_ReturnsTrue()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { Name = "My Collection" };
        HasValidNameRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyName_ReturnsFalse()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { Name = "" };
        HasValidNameRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithWhitespaceName_ReturnsFalse()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { Name = "   " };
        HasValidNameRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidNameRenameCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection name cannot be empty");
    }

    private sealed class RenameCollectionArgEntityFake : IRenameCollectionArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class NameNotTooLongRenameCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNameUnder100Chars_ReturnsTrue()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { Name = "Short Name" };
        NameNotTooLongRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNameExactly100Chars_ReturnsTrue()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { Name = new string('A', 100) };
        NameNotTooLongRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNameOver100Chars_ReturnsFalse()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { Name = new string('A', 101) };
        NameNotTooLongRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        NameNotTooLongRenameCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection name cannot exceed 100 characters");
    }

    private sealed class RenameCollectionArgEntityFake : IRenameCollectionArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class NameNotReservedRenameCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNonReservedName_ReturnsTrue()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { Name = "My Collection" };
        NameNotReservedRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithReservedNameLowercase_ReturnsFalse()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { Name = "default" };
        NameNotReservedRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithReservedNameUppercase_ReturnsFalse()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { Name = "DEFAULT" };
        NameNotReservedRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithReservedNameMixedCase_ReturnsFalse()
    {
        // Arrange
        RenameCollectionArgEntityFake args = new() { Name = "Default" };
        NameNotReservedRenameCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        NameNotReservedRenameCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection name 'default' is reserved");
    }

    private sealed class RenameCollectionArgEntityFake : IRenameCollectionArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }
}
