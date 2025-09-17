using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Validators;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.MtgDiscovery.Entry.Tests.Queries.Validators;

[TestClass]
public sealed class CardIdsArgEntityValidatorContainerTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithValidArgs_ReturnsValidResult()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = ["id1", "id2"] };
        CardIdsArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNullArgs_ReturnsInvalidResult()
    {
        // Arrange
        CardIdsArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> actual =
            await subject.Validate(null!).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeFalse();
        actual.FailureStatus().Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNullCardIds_ReturnsInvalidResult()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = null! };
        CardIdsArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithEmptyCardIds_ReturnsInvalidResult()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = [] };
        CardIdsArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidCardIds_ReturnsInvalidResult()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = ["id1", "", "  ", null!] };
        CardIdsArgEntityValidatorContainer subject = new();

        // Act
        IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> actual =
            await subject.Validate(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeFalse();
    }

    private sealed class FakeCardIdsArgEntity : ICardIdsArgEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }
}

[TestClass]
public sealed class IsNotNullCardIdsArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNonNullArgs_ReturnsTrue()
    {
        // Arrange
        FakeCardIdsArgEntity args = new();
        IsNotNullCardIdsArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNullArgs_ReturnsFalse()
    {
        // Arrange
        IsNotNullCardIdsArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(null!).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        IsNotNullCardIdsArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Provided object is null");
    }

    private sealed class FakeCardIdsArgEntity : ICardIdsArgEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }
}

[TestClass]
public sealed class IdsNotNullCardIdsArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNonNullCardIds_ReturnsTrue()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = [] };
        IdsNotNullCardIdsArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNullCardIds_ReturnsFalse()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = null! };
        IdsNotNullCardIdsArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        IdsNotNullCardIdsArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Provided list is null");
    }

    private sealed class FakeCardIdsArgEntity : ICardIdsArgEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }
}

[TestClass]
public sealed class HasIdsCardIdsArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNonEmptyCardIds_ReturnsTrue()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = ["id1"] };
        HasIdsCardIdsArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyCardIds_ReturnsFalse()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = [] };
        HasIdsCardIdsArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        HasIdsCardIdsArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Provided list is empty");
    }

    private sealed class FakeCardIdsArgEntity : ICardIdsArgEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }
}

[TestClass]
public sealed class ValidCardIdsArgEntityValidatorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithValidCardIds_ReturnsTrue()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = ["id1", "id2", "id3"] };
        ValidCardIdsArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithEmptyStringInCardIds_ReturnsFalse()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = ["id1", "", "id3"] };
        ValidCardIdsArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithWhitespaceInCardIds_ReturnsFalse()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = ["id1", "  ", "id3"] };
        ValidCardIdsArgEntityValidator.Validator subject = new();

        // Act
        bool actual = await subject.IsValid(args).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Message_AsSystemType_ReturnsExpectedMessage()
    {
        // Arrange
        ValidCardIdsArgEntityValidator.Message subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be("Provided list has invalid entries");
    }

    private sealed class FakeCardIdsArgEntity : ICardIdsArgEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }
}
