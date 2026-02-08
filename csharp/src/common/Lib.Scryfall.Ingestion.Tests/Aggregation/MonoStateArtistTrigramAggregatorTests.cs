using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using Lib.Scryfall.Ingestion.Aggregation;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Ingestion.Tests.Aggregation;

[TestClass]
[DoNotParallelize]
public sealed class MonoStateArtistTrigramAggregatorTests
{
    [TestMethod, TestCategory("unit")]
    public void TrackArtist_ValidName_GeneratesTrigrams()
    {
        // Arrange
        MonoStateArtistTrigramAggregator subject = new();
        subject.Clear();

        // Act
        subject.TrackArtist("artist-1", ["John Avon"]);

        // Assert
        List<IArtistTrigramAggregate> trigrams = [.. subject.GetTrigrams()];
        trigrams.Should().NotBeEmpty();
        trigrams.Select(t => t.Trigram()).Should().Contain("joh");
        trigrams.Select(t => t.Trigram()).Should().Contain("ohn");
    }

    [TestMethod, TestCategory("unit")]
    public void TrackArtist_NameShorterThanThreeLetters_ProducesNoTrigrams()
    {
        // Arrange
        MonoStateArtistTrigramAggregator subject = new();
        subject.Clear();

        // Act
        subject.TrackArtist("artist-1", ["Ab"]);

        // Assert
        subject.GetTrigrams().Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void TrackArtist_MultipleNames_ProcessesAll()
    {
        // Arrange
        MonoStateArtistTrigramAggregator subject = new();
        subject.Clear();

        // Act
        subject.TrackArtist("artist-1", ["John", "Avon"]);

        // Assert
        List<IArtistTrigramAggregate> trigrams = [.. subject.GetTrigrams()];
        trigrams.Select(t => t.Trigram()).Should().Contain("joh");
        trigrams.Select(t => t.Trigram()).Should().Contain("avo");
    }

    [TestMethod, TestCategory("unit")]
    public void TrackArtist_EmptyName_IsIgnored()
    {
        // Arrange
        MonoStateArtistTrigramAggregator subject = new();
        subject.Clear();

        // Act
        subject.TrackArtist("artist-1", [""]);

        // Assert
        subject.GetTrigrams().Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void TrackArtist_NullName_IsIgnored()
    {
        // Arrange
        MonoStateArtistTrigramAggregator subject = new();
        subject.Clear();

        // Act
        subject.TrackArtist("artist-1", [null!]);

        // Assert
        subject.GetTrigrams().Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void Clear_RemovesAllTrigrams()
    {
        // Arrange
        MonoStateArtistTrigramAggregator subject = new();
        subject.Clear();
        subject.TrackArtist("artist-1", ["John Avon"]);

        // Act
        subject.Clear();

        // Assert
        subject.GetTrigrams().Should().BeEmpty();
    }
}
