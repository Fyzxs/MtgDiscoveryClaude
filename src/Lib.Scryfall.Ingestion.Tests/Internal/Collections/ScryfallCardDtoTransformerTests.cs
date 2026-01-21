using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Models;
using Lib.Scryfall.Ingestion.Tests.Fakes;
using Lib.Scryfall.Ingestion.Transformers;
using Lib.Scryfall.Shared.Apis.Models;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.Tests.Internal.Collections;

[TestClass]
public sealed class ScryfallCardDtoTransformerTests
{
    [TestMethod]
    [TestCategory("unit")]
    public void Transform_ValidDto_ReturnsScryfallCard()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'id': 'test-card-id',
            'name': 'Lightning Bolt',
            'mana_cost': '{R}',
            'cmc': 1,
            'type_line': 'Instant',
            'oracle_text': 'Lightning Bolt deals 3 damage to any target.',
            'colors': ['R'],
            'rarity': 'common',
            'set': 'tst',
            'set_name': 'Test Set',
            'collector_number': '141',
            'power': null,
            'toughness': null,
            'artist': 'Test Artist',
            'flavor_text': 'The spark that ignites the powder keg.'
        }");
        ExtScryfallCardDto dto = new(rawData);
        IScryfallSet set = new ScryfallSetFake();
        ScryfallCardDtoTransformer transformer = new(set);

        // Act
        IScryfallCard actual = transformer.Transform(dto);

        // Assert
        _ = actual.Should().NotBeNull();
        _ = actual.Should().BeOfType<ScryfallCard>();
        _ = actual.Name().Should().Be("Lightning Bolt");
        _ = ((string)actual.Data().id).Should().Be("test-card-id");
        _ = ((string)actual.Data().mana_cost).Should().Be("{R}");
        _ = ((string)actual.Data().type_line).Should().Be("Instant");
        _ = ((string)actual.Data().oracle_text).Should().Be("Lightning Bolt deals 3 damage to any target.");
        _ = ((string)actual.Data().rarity).Should().Be("common");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Transform_CreatureDto_ReturnsCardWithPowerToughness()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'id': 'creature-id',
            'name': 'Test Creature',
            'mana_cost': '{2}{G}',
            'cmc': 3,
            'type_line': 'Creature — Beast',
            'oracle_text': 'Trample',
            'power': '4',
            'toughness': '3',
            'rarity': 'uncommon',
            'set': 'test'
        }");
        ExtScryfallCardDto dto = new(rawData);
        IScryfallSet set = new ScryfallSetFake();
        ScryfallCardDtoTransformer transformer = new(set);

        // Act
        IScryfallCard actual = transformer.Transform(dto);

        // Assert
        _ = actual.Should().NotBeNull();
        _ = actual.Name().Should().Be("Test Creature");
        _ = ((string)actual.Data().power).Should().Be("4");
        _ = ((string)actual.Data().toughness).Should().Be("3");
        _ = ((string)actual.Data().type_line).Should().Be("Creature — Beast");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Transform_MinimalDto_ReturnsScryfallCard()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'id': 'minimal-id',
            'name': 'Minimal Card',
            'set': 'test'
        }");
        ExtScryfallCardDto dto = new(rawData);
        IScryfallSet set = new ScryfallSetFake();
        ScryfallCardDtoTransformer transformer = new(set);

        // Act
        IScryfallCard actual = transformer.Transform(dto);

        // Assert
        _ = actual.Should().NotBeNull();
        _ = actual.Name().Should().Be("Minimal Card");
        _ = ((string)actual.Data().id).Should().Be("minimal-id");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Transform_DoubleFacedCard_ReturnsCardWithCardFaces()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'id': 'dfc-id',
            'name': 'Front Face // Back Face',
            'mana_cost': '',
            'type_line': 'Creature — Human // Creature — Werewolf',
            'set': 'test',
            'card_faces': [
                {
                    'name': 'Front Face',
                    'mana_cost': '{1}{G}',
                    'type_line': 'Creature — Human',
                    'oracle_text': 'Transform ability text',
                    'power': '2',
                    'toughness': '2'
                },
                {
                    'name': 'Back Face',
                    'mana_cost': '',
                    'type_line': 'Creature — Werewolf',
                    'oracle_text': 'Other side text',
                    'power': '4',
                    'toughness': '4'
                }
            ]
        }");
        ExtScryfallCardDto dto = new(rawData);
        IScryfallSet set = new ScryfallSetFake();
        ScryfallCardDtoTransformer transformer = new(set);

        // Act
        IScryfallCard actual = transformer.Transform(dto);

        // Assert
        _ = actual.Should().NotBeNull();
        _ = actual.Name().Should().Be("Front Face // Back Face");
        _ = ((string)actual.Data().card_faces[0].name).Should().Be("Front Face");
        _ = ((string)actual.Data().card_faces[1].name).Should().Be("Back Face");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Transform_MultipleCalls_ReturnsNewInstances()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'id': 'test-id',
            'name': 'Test Card',
            'set': 'test'
        }");
        ExtScryfallCardDto dto = new(rawData);
        IScryfallSet set = new ScryfallSetFake();
        ScryfallCardDtoTransformer transformer = new(set);

        // Act
        IScryfallCard result1 = transformer.Transform(dto);
        IScryfallCard result2 = transformer.Transform(dto);

        // Assert
        _ = result1.Should().NotBeNull();
        _ = result2.Should().NotBeNull();
        _ = result1.Should().NotBeSameAs(result2);
        _ = result1.Name().Should().Be(result2.Name());
    }
}
