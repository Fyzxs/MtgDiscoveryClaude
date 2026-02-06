using System.Linq;
using AwesomeAssertions;
using Lib.Scryfall.Ingestion.Aggregation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Ingestion.Tests.Aggregation;

[TestClass]
public sealed class ArtistAggregateTests
{
    [TestMethod, TestCategory("unit")]
    public void ArtistId_WhenConstructed_ReturnsProvidedId()
    {
        // Arrange & Act
        ArtistAggregate subject = new("artist-123");

        // Assert
        subject.ArtistId().Should().Be("artist-123");
    }

    [TestMethod, TestCategory("unit")]
    public void IsDirty_WhenNewlyConstructed_ReturnsFalse()
    {
        // Arrange & Act
        ArtistAggregate subject = new("artist-123");

        // Assert
        subject.IsDirty().Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void AddName_ValidName_SetsDirtyTrue()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");

        // Act
        subject.AddName("John Avon");

        // Assert
        subject.IsDirty().Should().BeTrue();
        subject.ArtistNames().Should().ContainSingle().Which.Should().Be("John Avon");
    }

    [TestMethod, TestCategory("unit")]
    public void AddName_DuplicateName_DoesNotReDirty()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");
        subject.AddName("John Avon");
        subject.MarkClean();

        // Act
        subject.AddName("John Avon");

        // Assert
        subject.IsDirty().Should().BeFalse();
        subject.ArtistNames().Should().HaveCount(1);
    }

    [TestMethod, TestCategory("unit")]
    public void AddName_NullName_IsIgnored()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");

        // Act
        subject.AddName(null!);

        // Assert
        subject.IsDirty().Should().BeFalse();
        subject.ArtistNames().Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void AddName_WhitespaceName_IsIgnored()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");

        // Act
        subject.AddName("   ");

        // Assert
        subject.IsDirty().Should().BeFalse();
        subject.ArtistNames().Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void AddCard_ValidCardId_SetsDirtyTrue()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");

        // Act
        subject.AddCard("card-456");

        // Assert
        subject.IsDirty().Should().BeTrue();
        subject.CardIds().Should().ContainSingle().Which.Should().Be("card-456");
    }

    [TestMethod, TestCategory("unit")]
    public void AddCard_DuplicateCardId_DoesNotReDirty()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");
        subject.AddCard("card-456");
        subject.MarkClean();

        // Act
        subject.AddCard("card-456");

        // Assert
        subject.IsDirty().Should().BeFalse();
        subject.CardIds().Should().HaveCount(1);
    }

    [TestMethod, TestCategory("unit")]
    public void AddCard_NullCardId_IsIgnored()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");

        // Act
        subject.AddCard(null!);

        // Assert
        subject.IsDirty().Should().BeFalse();
        subject.CardIds().Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void AddCard_WithData_StoresCardData()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");
        dynamic cardData = new { name = "Lightning Bolt" };

        // Act
        subject.AddCard("card-456", cardData);

        // Assert
        subject.IsDirty().Should().BeTrue();
        subject.CardIds().Should().ContainSingle().Which.Should().Be("card-456");
        subject.Cards().Should().HaveCount(1);
    }

    [TestMethod, TestCategory("unit")]
    public void AddCard_WithData_DuplicateCardId_IsIgnored()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");
        dynamic firstData = new { name = "First" };
        dynamic secondData = new { name = "Second" };
        subject.AddCard("card-456", firstData);
        subject.MarkClean();

        // Act
        subject.AddCard("card-456", secondData);

        // Assert
        subject.IsDirty().Should().BeFalse();
        subject.Cards().Should().HaveCount(1);
    }

    [TestMethod, TestCategory("unit")]
    public void AddCard_WithData_NullCardId_IsIgnored()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");

        // Act
        subject.AddCard(null!, new { });

        // Assert
        subject.IsDirty().Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void AddSet_ValidSetId_SetsDirtyTrue()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");

        // Act
        subject.AddSet("set-789");

        // Assert
        subject.IsDirty().Should().BeTrue();
        subject.SetIds().Should().ContainSingle().Which.Should().Be("set-789");
    }

    [TestMethod, TestCategory("unit")]
    public void AddSet_DuplicateSetId_DoesNotReDirty()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");
        subject.AddSet("set-789");
        subject.MarkClean();

        // Act
        subject.AddSet("set-789");

        // Assert
        subject.IsDirty().Should().BeFalse();
        subject.SetIds().Should().HaveCount(1);
    }

    [TestMethod, TestCategory("unit")]
    public void AddSet_NullSetId_IsIgnored()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");

        // Act
        subject.AddSet(null!);

        // Assert
        subject.IsDirty().Should().BeFalse();
        subject.SetIds().Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void MarkClean_AfterDirty_ResetsDirtyFlag()
    {
        // Arrange
        ArtistAggregate subject = new("artist-123");
        subject.AddName("John Avon");

        // Act
        subject.MarkClean();

        // Assert
        subject.IsDirty().Should().BeFalse();
        subject.ArtistNames().Should().HaveCount(1);
    }
}
