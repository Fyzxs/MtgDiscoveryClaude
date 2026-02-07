using System;
using AwesomeAssertions;
using Lib.Shared.Abstractions.Identifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Shared.Abstractions.Tests.Identifiers;

[TestClass]
public sealed class CardNameGuidGeneratorTests
{
    [TestMethod, TestCategory("unit")]
    public void GenerateGuid_ValidCardName_ReturnsDeterministicGuid()
    {
        // Arrange
        CardNameGuidGenerator subject = new();

        // Act
        CardNameGuid first = subject.GenerateGuid("Lightning Bolt");
        CardNameGuid second = subject.GenerateGuid("Lightning Bolt");

        // Assert
        first.AsSystemType().Should().Be(second.AsSystemType());
    }

    [TestMethod, TestCategory("unit")]
    public void GenerateGuid_SameNameDifferentCase_ReturnsSameGuid()
    {
        // Arrange
        CardNameGuidGenerator subject = new();

        // Act
        CardNameGuid lower = subject.GenerateGuid("lightning bolt");
        CardNameGuid upper = subject.GenerateGuid("LIGHTNING BOLT");
        CardNameGuid mixed = subject.GenerateGuid("Lightning Bolt");

        // Assert
        lower.AsSystemType().Should().Be(upper.AsSystemType());
        lower.AsSystemType().Should().Be(mixed.AsSystemType());
    }

    [TestMethod, TestCategory("unit")]
    public void GenerateGuid_DifferentNames_ReturnsDifferentGuids()
    {
        // Arrange
        CardNameGuidGenerator subject = new();

        // Act
        CardNameGuid bolt = subject.GenerateGuid("Lightning Bolt");
        CardNameGuid helix = subject.GenerateGuid("Lightning Helix");

        // Assert
        bolt.AsSystemType().Should().NotBe(helix.AsSystemType());
    }

    [TestMethod, TestCategory("unit")]
    public void GenerateGuid_NullInput_ThrowsArgumentException()
    {
        // Arrange
        CardNameGuidGenerator subject = new();

        // Act & Assert
        Action act = () => subject.GenerateGuid(null!);
        act.Should().Throw<ArgumentException>()
            .WithParameterName("cardName");
    }

    [TestMethod, TestCategory("unit")]
    public void GenerateGuid_EmptyString_ThrowsArgumentException()
    {
        // Arrange
        CardNameGuidGenerator subject = new();

        // Act & Assert
        Action act = () => subject.GenerateGuid("");
        act.Should().Throw<ArgumentException>()
            .WithParameterName("cardName");
    }

    [TestMethod, TestCategory("unit")]
    public void GenerateGuid_WhitespaceOnly_ThrowsArgumentException()
    {
        // Arrange
        CardNameGuidGenerator subject = new();

        // Act & Assert
        Action act = () => subject.GenerateGuid("   ");
        act.Should().Throw<ArgumentException>()
            .WithParameterName("cardName");
    }

    [TestMethod, TestCategory("unit")]
    public void GenerateGuid_ValidCardName_ReturnsNonEmptyGuid()
    {
        // Arrange
        CardNameGuidGenerator subject = new();

        // Act
        CardNameGuid actual = subject.GenerateGuid("Lightning Bolt");

        // Assert
        actual.AsSystemType().Should().NotBe(Guid.Empty);
    }
}
