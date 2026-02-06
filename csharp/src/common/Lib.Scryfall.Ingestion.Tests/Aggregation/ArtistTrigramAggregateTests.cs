using System.Linq;
using AwesomeAssertions;
using Lib.Scryfall.Ingestion.Aggregation;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Ingestion.Tests.Aggregation;

[TestClass]
public sealed class ArtistTrigramAggregateTests
{
    [TestMethod, TestCategory("unit")]
    public void Trigram_WhenConstructed_ReturnsProvidedTrigram()
    {
        // Arrange & Act
        ArtistTrigramAggregate subject = new("joh");

        // Assert
        subject.Trigram().Should().Be("joh");
    }

    [TestMethod, TestCategory("unit")]
    public void Entries_WhenNewlyConstructed_ReturnsEmpty()
    {
        // Arrange & Act
        ArtistTrigramAggregate subject = new("joh");

        // Assert
        subject.Entries().Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void AddArtist_NewArtist_CreatesEntry()
    {
        // Arrange
        ArtistTrigramAggregate subject = new("joh");

        // Act
        subject.AddArtist("artist-1", "John Avon", "johnavon", 0);

        // Assert
        IArtistTrigramEntry entry = subject.Entries().Should().ContainSingle().Which;
        entry.ArtistId().Should().Be("artist-1");
        entry.Name().Should().Be("John Avon");
        entry.Normalized().Should().Be("johnavon");
        entry.Positions().Should().ContainSingle().Which.Should().Be(0);
    }

    [TestMethod, TestCategory("unit")]
    public void AddArtist_SameArtistAndName_AddsPositionToExistingEntry()
    {
        // Arrange
        ArtistTrigramAggregate subject = new("joh");
        subject.AddArtist("artist-1", "John Avon", "johnavon", 0);

        // Act
        subject.AddArtist("artist-1", "John Avon", "johnavon", 3);

        // Assert
        IArtistTrigramEntry entry = subject.Entries().Should().ContainSingle().Which;
        entry.Positions().Should().HaveCount(2);
        entry.Positions().Should().ContainInOrder([0, 3]);
    }

    [TestMethod, TestCategory("unit")]
    public void AddArtist_DifferentArtists_CreatesSeparateEntries()
    {
        // Arrange
        ArtistTrigramAggregate subject = new("joh");

        // Act
        subject.AddArtist("artist-1", "John Avon", "johnavon", 0);
        subject.AddArtist("artist-2", "John Smith", "johnsmith", 0);

        // Assert
        subject.Entries().Should().HaveCount(2);
    }

    [TestMethod, TestCategory("unit")]
    public void AddArtist_SameArtistDifferentNames_CreatesSeparateEntries()
    {
        // Arrange
        ArtistTrigramAggregate subject = new("joh");

        // Act
        subject.AddArtist("artist-1", "John Avon", "johnavon", 0);
        subject.AddArtist("artist-1", "John A.", "johna", 0);

        // Assert
        subject.Entries().Should().HaveCount(2);
    }

    [TestMethod, TestCategory("unit")]
    public void Positions_ReturnsOrderedPositions()
    {
        // Arrange
        ArtistTrigramAggregate subject = new("joh");
        subject.AddArtist("artist-1", "John Avon", "johnavon", 5);
        subject.AddArtist("artist-1", "John Avon", "johnavon", 1);
        subject.AddArtist("artist-1", "John Avon", "johnavon", 3);

        // Act
        IArtistTrigramEntry entry = subject.Entries().First();

        // Assert
        entry.Positions().Should().ContainInOrder([1, 3, 5]);
    }

    [TestMethod, TestCategory("unit")]
    public void AddArtist_DuplicatePosition_DoesNotAddDuplicate()
    {
        // Arrange
        ArtistTrigramAggregate subject = new("joh");
        subject.AddArtist("artist-1", "John Avon", "johnavon", 0);

        // Act
        subject.AddArtist("artist-1", "John Avon", "johnavon", 0);

        // Assert
        IArtistTrigramEntry entry = subject.Entries().Should().ContainSingle().Which;
        entry.Positions().Should().ContainSingle();
    }
}
