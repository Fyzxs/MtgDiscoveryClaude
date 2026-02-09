using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Delete;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.MtgDiscovery.Entry.Tests.Commands.Collections.Validators.Delete;

[TestClass]
public sealed class DeleteCollectionArgEntityValidatorContainerTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithValidArgs_ReturnsValidResult()
    {
        // Arrange
        DeleteCollectionArgEntityFake args = new()
        {
            CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
        };
        DeleteCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IDeleteCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNullArgs_ReturnsInvalidResult()
    {
        // Arrange
        DeleteCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IDeleteCollectionItrEntity>> actual =
            await subject.Validate(null!).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyCollectionId_ReturnsInvalidResult()
    {
        // Arrange
        DeleteCollectionArgEntityFake args = new()
        {
            CollectionId = ""
        };
        DeleteCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IDeleteCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidGuidCollectionId_ReturnsInvalidResult()
    {
        // Arrange
        DeleteCollectionArgEntityFake args = new()
        {
            CollectionId = "not-a-valid-guid"
        };
        DeleteCollectionArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<IDeleteCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.IsNotValid().Should().BeTrue();
    }

    private sealed class DeleteCollectionArgEntityFake : IDeleteCollectionArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class IsNotNullDeleteCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNonNullArgs_ReturnsTrue()
    {
        // Arrange
        DeleteCollectionArgEntityFake args = new();
        IsNotNullDeleteCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNullArgs_ReturnsFalse()
    {
        // Arrange
        IsNotNullDeleteCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(null!).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        IsNotNullDeleteCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Delete collection argument cannot be null");
    }

    private sealed class DeleteCollectionArgEntityFake : IDeleteCollectionArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
    }
}

[TestClass]
public sealed class HasValidCollectionIdDeleteCollectionArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithValidGuid_ReturnsTrue()
    {
        // Arrange
        DeleteCollectionArgEntityFake args = new() { CollectionId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890" };
        HasValidCollectionIdDeleteCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyCollectionId_ReturnsFalse()
    {
        // Arrange
        DeleteCollectionArgEntityFake args = new() { CollectionId = "" };
        HasValidCollectionIdDeleteCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithInvalidGuid_ReturnsFalse()
    {
        // Arrange
        DeleteCollectionArgEntityFake args = new() { CollectionId = "not-a-guid" };
        HasValidCollectionIdDeleteCollectionArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasValidCollectionIdDeleteCollectionArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Collection ID must be a valid GUID");
    }

    private sealed class DeleteCollectionArgEntityFake : IDeleteCollectionArgEntity
    {
        public string CollectionId { get; init; } = string.Empty;
    }
}
