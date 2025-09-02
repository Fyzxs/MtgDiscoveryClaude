using System.Collections.Generic;
using Lib.Aggregator.Cards.Tests.Fakes;
using Lib.Aggregator.Scryfall.Shared.Models;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Tests.Entities;

[TestClass]
public sealed class CardItemCollectionItrEntityTests
{
    [TestMethod, TestCategory("unit")]
    public void Data_WhenSet_ReturnsCorrectValue()
    {
        // Arrange
        List<ICardItemItrEntity> expected = [
            new FakeCardItemItrEntity { Id = "card1", Name = "Card One" },
            new FakeCardItemItrEntity { Id = "card2", Name = "Card Two" }
        ];

        // Act
        CardItemCollectionItrEntity subject = new()
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
        CardItemCollectionItrEntity subject = new();

        // Assert
        subject.Data.Should().BeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void ImplementsICardItemCollectionItrEntity()
    {
        // Arrange
        CardItemCollectionItrEntity subject = new();

        // Act
        bool actual = subject is ICardItemCollectionItrEntity;

        // Assert
        actual.Should().BeTrue();
    }
}
