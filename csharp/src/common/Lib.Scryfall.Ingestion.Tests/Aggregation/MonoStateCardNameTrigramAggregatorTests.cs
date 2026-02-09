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
public sealed class MonoStateCardNameTrigramAggregatorTests
{
    [TestMethod, TestCategory("unit")]
    public void Track_CardWithName_GeneratesTrigrams()
    {
        // Arrange
        MonoStateCardNameTrigramAggregator subject = new();
        subject.Clear();
        ScryfallCardFake card = new(name: "Bolt");

        // Act
        subject.Track(card);

        // Assert
        List<ICardNameTrigramAggregate> trigrams = [.. subject.GetTrigrams()];
        trigrams.Should().HaveCount(2);
        trigrams.Select(t => t.Trigram()).Should().Contain("bol");
        trigrams.Select(t => t.Trigram()).Should().Contain("olt");
    }

    [TestMethod, TestCategory("unit")]
    public void Track_CardWithFlavorName_ProcessesBothNames()
    {
        // Arrange
        MonoStateCardNameTrigramAggregator subject = new();
        subject.Clear();
        ScryfallCardFake card = new(name: "Bolt", flavorName: "Zap!");

        // Act
        subject.Track(card);

        // Assert
        List<ICardNameTrigramAggregate> trigrams = [.. subject.GetTrigrams()];
        trigrams.Select(t => t.Trigram()).Should().Contain("bol");
        trigrams.Select(t => t.Trigram()).Should().Contain("olt");
    }

    [TestMethod, TestCategory("unit")]
    public void Track_CardNameShorterThanThreeLetters_ProducesNoTrigrams()
    {
        // Arrange
        MonoStateCardNameTrigramAggregator subject = new();
        subject.Clear();
        ScryfallCardFake card = new(name: "Ab");

        // Act
        subject.Track(card);

        // Assert
        subject.GetTrigrams().Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void Track_CardNameWithNonLetters_StripsNonLetters()
    {
        // Arrange
        MonoStateCardNameTrigramAggregator subject = new();
        subject.Clear();
        ScryfallCardFake card = new(name: "A-B-C-D");

        // Act
        subject.Track(card);

        // Assert
        List<ICardNameTrigramAggregate> trigrams = [.. subject.GetTrigrams()];
        trigrams.Should().HaveCount(2);
        trigrams.Select(t => t.Trigram()).Should().Contain("abc");
        trigrams.Select(t => t.Trigram()).Should().Contain("bcd");
    }

    [TestMethod, TestCategory("unit")]
    public void Clear_RemovesAllTrigrams()
    {
        // Arrange
        MonoStateCardNameTrigramAggregator subject = new();
        subject.Clear();
        ScryfallCardFake card = new(name: "Lightning Bolt");
        subject.Track(card);

        // Act
        subject.Clear();

        // Assert
        subject.GetTrigrams().Should().BeEmpty();
    }
}
