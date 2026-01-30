using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Visibility;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.MtgDiscovery.Entry.Tests.Commands.Collections.Validators.Visibility;

[TestClass]
public sealed class UpdateCollectionVisibilityArgEntityValidatorContainerTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithValidArgs_ReturnsValidResult()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            Visibility = "public"
        };
        UpdateCollectionVisibilityArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUpdateCollectionVisibilityItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNullArgs_ReturnsInvalidResult()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUpdateCollectionVisibilityItrEntity>> actual =
            await subject.Validate(null!).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyCollectionId_ReturnsInvalidResult()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new()
        {
            CollectionId = "",
            Visibility = "private"
        };
        UpdateCollectionVisibilityArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUpdateCollectionVisibilityItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidGuidCollectionId_ReturnsInvalidResult()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new()
        {
            CollectionId = "not-a-valid-guid",
            Visibility = "private"
        };
        UpdateCollectionVisibilityArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUpdateCollectionVisibilityItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithPrivateVisibility_ReturnsValidResult()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            Visibility = "private"
        };
        UpdateCollectionVisibilityArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUpdateCollectionVisibilityItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithPublicVisibility_ReturnsValidResult()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            Visibility = "public"
        };
        UpdateCollectionVisibilityArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUpdateCollectionVisibilityItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidVisibility_ReturnsInvalidResult()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            Visibility = "unknown"
        };
        UpdateCollectionVisibilityArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUpdateCollectionVisibilityItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyVisibility_ReturnsInvalidResult()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            Visibility = ""
        };
        UpdateCollectionVisibilityArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IUpdateCollectionVisibilityItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsNotValid().Should().BeTrue();
    }

    private sealed class UpdateCollectionVisibilityArgEntityFake : IUpdateCollectionVisibilityArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string Visibility { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class IsNotNullUpdateCollectionVisibilityArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNonNullArgs_ReturnsTrue()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new();
        IsNotNullUpdateCollectionVisibilityArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNullArgs_ReturnsFalse()
    {
        // Arrange
        IsNotNullUpdateCollectionVisibilityArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(null!).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        IsNotNullUpdateCollectionVisibilityArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Update collection visibility argument cannot be null");
    }

    private sealed class UpdateCollectionVisibilityArgEntityFake : IUpdateCollectionVisibilityArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string Visibility { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidCollectionIdUpdateCollectionVisibilityArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithValidGuid_ReturnsTrue()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new() { CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890" };
        HasValidCollectionIdUpdateCollectionVisibilityArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyCollectionId_ReturnsFalse()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new() { CollectionId = "" };
        HasValidCollectionIdUpdateCollectionVisibilityArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidGuid_ReturnsFalse()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new() { CollectionId = "not-a-guid" };
        HasValidCollectionIdUpdateCollectionVisibilityArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidCollectionIdUpdateCollectionVisibilityArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection ID must be a valid GUID");
    }

    private sealed class UpdateCollectionVisibilityArgEntityFake : IUpdateCollectionVisibilityArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string Visibility { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidVisibilityUpdateCollectionVisibilityArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithPrivateVisibility_ReturnsTrue()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new() { Visibility = "private" };
        HasValidVisibilityUpdateCollectionVisibilityArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithPublicVisibility_ReturnsTrue()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new() { Visibility = "public" };
        HasValidVisibilityUpdateCollectionVisibilityArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidVisibility_ReturnsFalse()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new() { Visibility = "unknown" };
        HasValidVisibilityUpdateCollectionVisibilityArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyVisibility_ReturnsFalse()
    {
        // Arrange
        UpdateCollectionVisibilityArgEntityFake args = new() { Visibility = "" };
        HasValidVisibilityUpdateCollectionVisibilityArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidVisibilityUpdateCollectionVisibilityArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Visibility must be either 'private' or 'public'");
    }

    private sealed class UpdateCollectionVisibilityArgEntityFake : IUpdateCollectionVisibilityArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
        public string Visibility { get; init; } = string.Empty;
    }
}
