using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.MtgDiscovery.Entry.Tests.Commands.Collections.Validators;

[TestClass]
public sealed class CreateCollectionArgEntityValidatorContainerTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithValidArgs_ReturnsValidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = "My Custom Collection",
            Type = "custom",
            Visibility = "private"
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNullArgs_ReturnsInvalidResult()
    {
        // Arrange
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(null!).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyName_ReturnsInvalidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = "",
            Type = "custom",
            Visibility = "private"
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithWhitespaceName_ReturnsInvalidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = "   ",
            Type = "custom",
            Visibility = "private"
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNameTooLong_ReturnsInvalidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = new string('A', 101),
            Type = "custom",
            Visibility = "private"
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNameExactly100Chars_ReturnsValidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = new string('A', 100),
            Type = "custom",
            Visibility = "private"
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithReservedNameDefault_ReturnsInvalidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = "default",
            Type = "custom",
            Visibility = "private"
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithReservedNameDefaultUpperCase_ReturnsInvalidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = "DEFAULT",
            Type = "custom",
            Visibility = "private"
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidType_ReturnsInvalidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = "My Collection",
            Type = "invalid-type",
            Visibility = "private"
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithTypeCube_ReturnsValidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = "My Cube",
            Type = "cube",
            Visibility = "private"
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithTypeTrade_ReturnsValidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = "My Trades",
            Type = "trade",
            Visibility = "public"
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidVisibility_ReturnsInvalidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = "My Collection",
            Type = "custom",
            Visibility = "unknown"
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyVisibility_ReturnsValidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = "My Collection",
            Type = "custom",
            Visibility = ""
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNullVisibility_ReturnsValidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = "My Collection",
            Type = "custom",
            Visibility = null!
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithPublicVisibility_ReturnsValidResult()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new()
        {
            Name = "My Collection",
            Type = "custom",
            Visibility = "public"
        };
        CreateCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
    }

    private sealed class CreateCollectionArgEntityFake : ICreateCollectionArgEntity
    {
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Visibility { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class IsNotNullCreateCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNonNullArgs_ReturnsTrue()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new();
        IsNotNullCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNullArgs_ReturnsFalse()
    {
        // Arrange
        IsNotNullCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(null!).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        IsNotNullCreateCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Create collection argument cannot be null");
    }

    private sealed class CreateCollectionArgEntityFake : ICreateCollectionArgEntity
    {
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Visibility { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidNameCreateCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithValidName_ReturnsTrue()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Name = "My Collection" };
        HasValidNameCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyName_ReturnsFalse()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Name = "" };
        HasValidNameCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithWhitespaceName_ReturnsFalse()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Name = "   " };
        HasValidNameCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidNameCreateCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection name cannot be empty");
    }

    private sealed class CreateCollectionArgEntityFake : ICreateCollectionArgEntity
    {
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Visibility { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class NameNotTooLongCreateCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNameUnder100Chars_ReturnsTrue()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Name = "Short Name" };
        NameNotTooLongCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNameExactly100Chars_ReturnsTrue()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Name = new string('A', 100) };
        NameNotTooLongCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNameOver100Chars_ReturnsFalse()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Name = new string('A', 101) };
        NameNotTooLongCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        NameNotTooLongCreateCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection name cannot exceed 100 characters");
    }

    private sealed class CreateCollectionArgEntityFake : ICreateCollectionArgEntity
    {
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Visibility { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class NameNotReservedCreateCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNonReservedName_ReturnsTrue()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Name = "My Collection" };
        NameNotReservedCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithReservedNameLowercase_ReturnsFalse()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Name = "default" };
        NameNotReservedCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithReservedNameUppercase_ReturnsFalse()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Name = "DEFAULT" };
        NameNotReservedCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithReservedNameMixedCase_ReturnsFalse()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Name = "Default" };
        NameNotReservedCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        NameNotReservedCreateCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection name 'default' is reserved");
    }

    private sealed class CreateCollectionArgEntityFake : ICreateCollectionArgEntity
    {
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Visibility { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidTypeCreateCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithTypeCustom_ReturnsTrue()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Type = "custom" };
        HasValidTypeCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithTypeCube_ReturnsTrue()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Type = "cube" };
        HasValidTypeCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithTypeTrade_ReturnsTrue()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Type = "trade" };
        HasValidTypeCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidType_ReturnsFalse()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Type = "invalid" };
        HasValidTypeCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyType_ReturnsFalse()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Type = "" };
        HasValidTypeCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidTypeCreateCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection type must be 'custom', 'cube', or 'trade'");
    }

    private sealed class CreateCollectionArgEntityFake : ICreateCollectionArgEntity
    {
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Visibility { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidVisibilityCreateCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithPrivateVisibility_ReturnsTrue()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Visibility = "private" };
        HasValidVisibilityCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithPublicVisibility_ReturnsTrue()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Visibility = "public" };
        HasValidVisibilityCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyVisibility_ReturnsTrue()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Visibility = "" };
        HasValidVisibilityCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNullVisibility_ReturnsTrue()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Visibility = null! };
        HasValidVisibilityCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidVisibility_ReturnsFalse()
    {
        // Arrange
        CreateCollectionArgEntityFake args = new() { Visibility = "unknown" };
        HasValidVisibilityCreateCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidVisibilityCreateCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection visibility must be 'private' or 'public'");
    }

    private sealed class CreateCollectionArgEntityFake : ICreateCollectionArgEntity
    {
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Visibility { get; init; } = string.Empty;
    }
}
