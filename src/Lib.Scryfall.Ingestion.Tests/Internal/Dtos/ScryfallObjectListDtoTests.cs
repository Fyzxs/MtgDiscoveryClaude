using Lib.Scryfall.Ingestion.Internal.Dtos;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.Tests.Internal.Dtos;

[TestClass]
public sealed class ScryfallObjectListDtoTests
{
    [TestMethod]
    [TestCategory("unit")]
    public void Data_WithDataInRawData_ReturnsData()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'data': [
                { 'id': '1', 'name': 'Item 1' },
                { 'id': '2', 'name': 'Item 2' }
            ],
            'has_more': true,
            'next_page': 'https://api.scryfall.com/page2',
            'total_cards': 100
        }");
        ScryfallObjectListDto subject = new(rawData);

        // Act
        dynamic actual = subject.Data;

        // Assert
        _ = ((object)actual).Should().NotBeNull();
        _ = ((object)actual[0].id.ToString()).Should().Be("1");
        _ = ((object)actual[0].name.ToString()).Should().Be("Item 1");
        _ = ((object)actual[1].id.ToString()).Should().Be("2");
        _ = ((object)actual[1].name.ToString()).Should().Be("Item 2");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void HasMore_WhenTrue_ReturnsTrue()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'data': [],
            'has_more': true,
            'next_page': 'https://api.scryfall.com/page2',
            'total_cards': 100
        }");
        ScryfallObjectListDto subject = new(rawData);

        // Act
        bool actual = subject.HasMore;

        // Assert
        _ = actual.Should().BeTrue();
    }

    [TestMethod]
    [TestCategory("unit")]
    public void HasMore_WhenFalse_ReturnsFalse()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'data': [],
            'has_more': false,
            'next_page': null,
            'total_cards': 10
        }");
        ScryfallObjectListDto subject = new(rawData);

        // Act
        bool actual = subject.HasMore;

        // Assert
        _ = actual.Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("unit")]
    public void HasMore_WhenNull_ReturnsFalse()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'data': [],
            'next_page': null,
            'total_cards': 0
        }");
        ScryfallObjectListDto subject = new(rawData);

        // Act
        bool actual = subject.HasMore;

        // Assert
        _ = actual.Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("unit")]
    public void NextPage_WithValue_ReturnsUrl()
    {
        // Arrange
        const string expectedUrl = "https://api.scryfall.com/page2";
        dynamic rawData = JsonConvert.DeserializeObject($@"{{
            'data': [],
            'has_more': true,
            'next_page': '{expectedUrl}',
            'total_cards': 100
        }}");
        ScryfallObjectListDto subject = new(rawData);

        // Act
        string actual = subject.NextPage;

        // Assert
        _ = actual.Should().Be(expectedUrl);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void NextPage_WhenNull_ReturnsNull()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'data': [],
            'has_more': false,
            'next_page': null,
            'total_cards': 10
        }");
        ScryfallObjectListDto subject = new(rawData);

        // Act
        string actual = subject.NextPage;

        // Assert
        _ = actual.Should().BeNull();
    }

    [TestMethod]
    [TestCategory("unit")]
    public void TotalCards_WithValue_ReturnsCount()
    {
        // Arrange
        const int expectedCount = 42;
        dynamic rawData = JsonConvert.DeserializeObject($@"{{
            'data': [],
            'has_more': false,
            'next_page': null,
            'total_cards': {expectedCount}
        }}");
        ScryfallObjectListDto subject = new(rawData);

        // Act
        int actual = subject.TotalCards;

        // Assert
        _ = actual.Should().Be(expectedCount);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void TotalCards_WhenNull_ReturnsZero()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'data': [],
            'has_more': false,
            'next_page': null
        }");
        ScryfallObjectListDto subject = new(rawData);

        // Act
        int actual = subject.TotalCards;

        // Assert
        _ = actual.Should().Be(0);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Data_WhenNull_ReturnsNull()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'has_more': false,
            'next_page': null,
            'total_cards': 0
        }");
        ScryfallObjectListDto subject = new(rawData);

        // Act
        dynamic actual = subject.Data;

        // Assert
        _ = ((object)actual).Should().BeNull();
    }
}
