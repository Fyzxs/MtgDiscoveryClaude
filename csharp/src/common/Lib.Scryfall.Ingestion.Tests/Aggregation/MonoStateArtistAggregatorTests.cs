using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using Lib.Scryfall.Ingestion.Aggregation;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Lib.Scryfall.Ingestion.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Ingestion.Tests.Aggregation;

[TestClass]
[DoNotParallelize]
public sealed class MonoStateArtistAggregatorTests
{
    [TestMethod, TestCategory("unit")]
    public void Track_CardWithArtist_CreatesArtistAggregate()
    {
        // Arrange
        MonoStateArtistAggregator subject = new();
        subject.Clear();
        ScryfallCardFake card = new(
            id: "card-1",
            artistIdNamePairs: [new ArtistIdNamePairFake("artist-1", "John Avon")]);

        // Act
        subject.Track(card);

        // Assert
        subject.GetArtists().Should().ContainSingle();
        IArtistAggregate artist = subject.GetArtists().First();
        artist.ArtistId().Should().Be("artist-1");
        artist.ArtistNames().Should().ContainSingle().Which.Should().Be("John Avon");
        artist.CardIds().Should().ContainSingle().Which.Should().Be("card-1");
    }

    [TestMethod, TestCategory("unit")]
    public void Track_MultipleCardsWithSameArtist_AggregatesIntoOne()
    {
        // Arrange
        MonoStateArtistAggregator subject = new();
        subject.Clear();
        ScryfallCardFake card1 = new(
            id: "card-1",
            artistIdNamePairs: [new ArtistIdNamePairFake("artist-1", "John Avon")]);
        ScryfallCardFake card2 = new(
            id: "card-2",
            artistIdNamePairs: [new ArtistIdNamePairFake("artist-1", "John Avon")]);

        // Act
        subject.Track(card1);
        subject.Track(card2);

        // Assert
        subject.GetArtists().Should().ContainSingle();
        IArtistAggregate artist = subject.GetArtists().First();
        artist.CardIds().Should().HaveCount(2);
    }

    [TestMethod, TestCategory("unit")]
    public void GetDirtyArtists_AfterTrack_ReturnsDirtyOnly()
    {
        // Arrange
        MonoStateArtistAggregator subject = new();
        subject.Clear();
        ScryfallCardFake card1 = new(
            id: "card-1",
            artistIdNamePairs: [new ArtistIdNamePairFake("artist-1", "John Avon")]);
        ScryfallCardFake card2 = new(
            id: "card-2",
            artistIdNamePairs: [new ArtistIdNamePairFake("artist-2", "Mark Poole")]);
        subject.Track(card1);
        subject.Track(card2);
        subject.MarkAllClean();

        ScryfallCardFake card3 = new(
            id: "card-3",
            artistIdNamePairs: [new ArtistIdNamePairFake("artist-1", "John Avon")]);
        subject.Track(card3);

        // Act
        List<IArtistAggregate> dirty = subject.GetDirtyArtists().ToList();

        // Assert
        dirty.Should().ContainSingle();
        dirty.First().ArtistId().Should().Be("artist-1");
    }

    [TestMethod, TestCategory("unit")]
    public void MarkAllClean_ResetsAllDirtyFlags()
    {
        // Arrange
        MonoStateArtistAggregator subject = new();
        subject.Clear();
        ScryfallCardFake card = new(
            id: "card-1",
            artistIdNamePairs: [new ArtistIdNamePairFake("artist-1", "John Avon")]);
        subject.Track(card);

        // Act
        subject.MarkAllClean();

        // Assert
        subject.GetDirtyArtists().Should().BeEmpty();
        subject.GetArtists().Should().ContainSingle();
    }

    [TestMethod, TestCategory("unit")]
    public void Clear_RemovesAllArtists()
    {
        // Arrange
        MonoStateArtistAggregator subject = new();
        subject.Clear();
        ScryfallCardFake card = new(
            id: "card-1",
            artistIdNamePairs: [new ArtistIdNamePairFake("artist-1", "John Avon")]);
        subject.Track(card);

        // Act
        subject.Clear();

        // Assert
        subject.GetArtists().Should().BeEmpty();
    }
}
