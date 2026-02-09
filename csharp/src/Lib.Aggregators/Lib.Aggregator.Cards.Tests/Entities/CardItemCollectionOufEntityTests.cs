using System.Collections.Generic;
using Lib.Aggregator.Cards.Tests.Fakes;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Tests.Entities;

[TestClass]
public sealed class CardItemCollectionOufEntityTests
{
    [TestMethod, TestCategory("unit")]
    public void Data_WhenSet_ReturnsCorrectValue()
    {
        // Arrange
        List<ICardItemOufEntity> expected = [
            new CardItemOufEntityFake { Id = "card1", Name = "Card One" },
            new CardItemOufEntityFake { Id = "card2", Name = "Card Two" }
        ];

        // Act
        CardItemCollectionOufEntity subject = new()
        {
            Data = expected
        };

        // Assert
        subject.Data.Should().BeSameAs(expected);
        subject.Data.Should().HaveCount(2);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_CreatesInstanceWithNullData()
    {
        // Arrange & Act
        CardItemCollectionOufEntity subject = new();

        // Assert
        subject.Data.Should().BeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void ImplementsICardItemCollectionOufEntity()
    {
        // Arrange
        CardItemCollectionOufEntity subject = new();

        // Act
        bool actual = subject is ICardItemCollectionOufEntity;

        // Assert
        actual.Should().BeTrue();
    }
}
