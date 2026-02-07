using System.Linq;
using AwesomeAssertions;
using Lib.Scryfall.Ingestion.Aggregation;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Ingestion.Tests.Aggregation;

[TestClass]
public sealed class CardNameTrigramAggregateTests
{
    [TestMethod, TestCategory("unit")]
    public void Trigram_WhenConstructed_ReturnsProvidedTrigram()
    {
        // Arrange & Act
        CardNameTrigramAggregate subject = new("lig");

        // Assert
        subject.Trigram().Should().Be("lig");
    }

    [TestMethod, TestCategory("unit")]
    public void Entries_WhenNewlyConstructed_ReturnsEmpty()
    {
        // Arrange & Act
        CardNameTrigramAggregate subject = new("lig");

        // Assert
        subject.Entries().Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void AddCard_NewName_CreatesEntry()
    {
        // Arrange
        CardNameTrigramAggregate subject = new("lig");

        // Act
        subject.AddCard("Lightning Bolt", "lightningbolt", 0);

        // Assert
        ICardNameTrigramEntry entry = subject.Entries().Should().ContainSingle().Which;
        entry.Name().Should().Be("Lightning Bolt");
        entry.Normalized().Should().Be("lightningbolt");
        entry.Positions().Should().ContainSingle().Which.Should().Be(0);
    }

    [TestMethod, TestCategory("unit")]
    public void AddCard_SameNameDifferentPositions_AddsToExistingEntry()
    {
        // Arrange
        CardNameTrigramAggregate subject = new("lig");
        subject.AddCard("Lightning Bolt", "lightningbolt", 0);

        // Act
        subject.AddCard("Lightning Bolt", "lightningbolt", 5);

        // Assert
        ICardNameTrigramEntry entry = subject.Entries().Should().ContainSingle().Which;
        entry.Positions().Should().HaveCount(2);
        entry.Positions().Should().ContainInOrder([0, 5]);
    }

    [TestMethod, TestCategory("unit")]
    public void AddCard_DifferentNames_CreatesSeparateEntries()
    {
        // Arrange
        CardNameTrigramAggregate subject = new("lig");

        // Act
        subject.AddCard("Lightning Bolt", "lightningbolt", 0);
        subject.AddCard("Lightning Helix", "lightninghelix", 0);

        // Assert
        subject.Entries().Should().HaveCount(2);
    }

    [TestMethod, TestCategory("unit")]
    public void AddCard_DuplicatePosition_DoesNotAddDuplicate()
    {
        // Arrange
        CardNameTrigramAggregate subject = new("lig");
        subject.AddCard("Lightning Bolt", "lightningbolt", 0);

        // Act
        subject.AddCard("Lightning Bolt", "lightningbolt", 0);

        // Assert
        ICardNameTrigramEntry entry = subject.Entries().Should().ContainSingle().Which;
        entry.Positions().Should().ContainSingle();
    }

    [TestMethod, TestCategory("unit")]
    public void Positions_ReturnsOrderedPositions()
    {
        // Arrange
        CardNameTrigramAggregate subject = new("lig");
        subject.AddCard("Lightning Bolt", "lightningbolt", 5);
        subject.AddCard("Lightning Bolt", "lightningbolt", 1);
        subject.AddCard("Lightning Bolt", "lightningbolt", 3);

        // Act
        ICardNameTrigramEntry entry = subject.Entries().First();

        // Assert
        entry.Positions().Should().ContainInOrder([1, 3, 5]);
    }
}
