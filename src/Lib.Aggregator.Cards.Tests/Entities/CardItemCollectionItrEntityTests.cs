using System.Collections.Generic;
using Lib.Aggregator.Cards.Tests.Fakes;

namespace Lib.Aggregator.Cards.Tests.Entities;

[TestClass]
public sealed class CardItemCollectionOufEntityTests
{
    [TestMethod, TestCategory("unit")]
    public void Data_WhenSet_ReturnsCorrectValue()
    {
        // Arrange
        List<ICardItemItrEntity> expected = [
            new CardItemItrEntityFake { Id = "card1", Name = "Card One" },
            new CardItemItrEntityFake { Id = "card2", Name = "Card Two" }
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
