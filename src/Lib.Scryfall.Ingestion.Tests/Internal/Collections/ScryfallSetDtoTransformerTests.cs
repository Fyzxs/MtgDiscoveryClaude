using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Models;
using Lib.Scryfall.Ingestion.Transformers;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.Tests.Internal.Collections;

[TestClass]
public sealed class ScryfallSetDtoTransformerTests
{
    [TestMethod]
    [TestCategory("unit")]
    public void Transform_ValidDto_ReturnsScryfallSet()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'id': 'test-set-id',
            'code': 'TST',
            'name': 'Test Set',
            'search_uri': 'https://api.scryfall.com/cards/search?q=set:tst',
            'released_at': '2024-01-01',
            'set_type': 'expansion',
            'card_count': 250,
            'icon_svg_uri': 'https://svgs.scryfall.io/sets/tst.svg'
        }");
        ExtScryfallSetDto dto = new(rawData);
        ScryfallSetDtoTransformer transformer = new(NullLogger.Instance);

        // Act
        IScryfallSet actual = transformer.Transform(dto);

        // Assert
        _ = actual.Should().NotBeNull();
        _ = actual.Should().BeOfType<ScryfallSet>();
        _ = actual.Name().Should().Be("Test Set");
        _ = ((string)actual.Data().id).Should().Be("test-set-id");
        _ = ((string)actual.Data().code).Should().Be("TST");
        _ = ((string)actual.Data().set_type).Should().Be("expansion");
        _ = ((int)actual.Data().card_count).Should().Be(250);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Transform_MinimalDto_ReturnsScryfallSet()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'id': 'minimal-id',
            'code': 'MIN',
            'name': 'Minimal Set'
        }");
        ExtScryfallSetDto dto = new(rawData);
        ScryfallSetDtoTransformer transformer = new(NullLogger.Instance);

        // Act
        IScryfallSet actual = transformer.Transform(dto);

        // Assert
        _ = actual.Should().NotBeNull();
        _ = actual.Name().Should().Be("Minimal Set");
        _ = ((string)actual.Data().id).Should().Be("minimal-id");
        _ = ((string)actual.Data().code).Should().Be("MIN");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Transform_DtoWithNullFields_ReturnsScryfallSet()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'id': 'null-fields-id',
            'code': 'NUL',
            'name': 'Null Fields Set',
            'released_at': null,
            'set_type': null,
            'card_count': null
        }");
        ExtScryfallSetDto dto = new(rawData);
        ScryfallSetDtoTransformer transformer = new(NullLogger.Instance);

        // Act
        IScryfallSet actual = transformer.Transform(dto);

        // Assert
        _ = actual.Should().NotBeNull();
        _ = actual.Name().Should().Be("Null Fields Set");
        _ = ((string)actual.Data().id).Should().Be("null-fields-id");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Transform_MultipleCalls_ReturnsNewInstances()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'id': 'test-id',
            'code': 'TST',
            'name': 'Test Set'
        }");
        ExtScryfallSetDto dto = new(rawData);
        ScryfallSetDtoTransformer transformer = new(NullLogger.Instance);

        // Act
        IScryfallSet result1 = transformer.Transform(dto);
        IScryfallSet result2 = transformer.Transform(dto);

        // Assert
        _ = result1.Should().NotBeNull();
        _ = result2.Should().NotBeNull();
        _ = result1.Should().NotBeSameAs(result2);
        _ = result1.Name().Should().Be(result2.Name());
    }
}
