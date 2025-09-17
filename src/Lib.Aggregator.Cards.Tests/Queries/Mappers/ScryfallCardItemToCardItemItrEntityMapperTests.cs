using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Cards.Tests.Fakes;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities;
using Newtonsoft.Json.Linq;

namespace Lib.Aggregator.Cards.Tests.Queries.Mappers;

[TestClass]
public sealed class ScryfallCardItemToCardItemItrEntityMapperTests
{
    [TestMethod, TestCategory("unit")]
    public void Map_WithNullInput_ReturnsNull()
    {
        // Arrange
        ScryfallCardItemToCardItemItrEntityMapper subject = new();

        // Act
        ICardItemItrEntity actual = subject.Map(null);

        // Assert
        actual.Should().BeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void Map_WithValidScryfallCard_ReturnsCardItemItrEntity()
    {
        // Arrange
        dynamic testData = new JObject
        {
            ["id"] = "test-id",
            ["name"] = "Test Card",
            ["oracle_id"] = "oracle-123",
            ["lang"] = "en",
            ["released_at"] = "2023-01-01",
            ["uri"] = "https://test.com/card",
            ["scryfall_uri"] = "https://scryfall.com/card",
            ["layout"] = "normal",
            ["highres_image"] = true,
            ["image_status"] = "highres_scan",
            ["mana_cost"] = "{2}{U}",
            ["cmc"] = 3.0m,
            ["type_line"] = "Creature — Test",
            ["oracle_text"] = "Test ability",
            ["reserved"] = false,
            ["foil"] = true,
            ["nonfoil"] = false,
            ["oversized"] = false,
            ["promo"] = false,
            ["reprint"] = false,
            ["variation"] = false,
            ["set"] = "TST",
            ["set_name"] = "Test Set",
            ["set_type"] = "expansion",
            ["collector_number"] = "42",
            ["digital"] = false,
            ["rarity"] = "rare",
            ["artist"] = "Test Artist",
            ["border_color"] = "black",
            ["frame"] = "2015",
            ["full_art"] = false,
            ["textless"] = false,
            ["booster"] = true,
            ["story_spotlight"] = false
        };

        ScryfallCardExtArg scryfallCard = FakeScryfallCardItemFactory.Create(testData);
        ScryfallCardItemToCardItemItrEntityMapper subject = new();

        // Act
        ICardItemItrEntity actual = subject.Map(scryfallCard);

        // Assert
        actual.Should().NotBeNull();
        actual.Should().BeOfType<CardItemItrEntity>();
        actual.Id.Should().Be("test-id");
        actual.Name.Should().Be("Test Card");
        actual.OracleId.Should().Be("oracle-123");
        actual.Lang.Should().Be("en");
        actual.ManaCost.Should().Be("{2}{U}");
        actual.Cmc.Should().Be(3.0m);
        actual.TypeLine.Should().Be("Creature — Test");
        actual.Foil.Should().BeTrue();
        actual.NonFoil.Should().BeFalse();
        actual.SetCode.Should().Be("TST");
        actual.CollectorNumber.Should().Be("42");
        actual.Rarity.Should().Be("rare");
    }

    [TestMethod, TestCategory("unit")]
    public void Map_WithEmptyData_ReturnsEntityWithNullProperties()
    {
        // Arrange
        ScryfallCardExtArg scryfallCard = FakeScryfallCardItemFactory.Create(new JObject());
        ScryfallCardItemToCardItemItrEntityMapper subject = new();

        // Act
        ICardItemItrEntity actual = subject.Map(scryfallCard);

        // Assert
        actual.Should().NotBeNull();
        actual.Id.Should().BeNull();
        actual.Name.Should().BeNull();
        actual.Colors.Should().BeNull();
        actual.ColorIdentity.Should().BeNull();
        actual.Keywords.Should().BeNull();
        actual.HighResImage.Should().BeFalse();
        actual.Reserved.Should().BeFalse();
        actual.Digital.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Map_ReturnsNewInstanceEachTime()
    {
        // Arrange
        ScryfallCardExtArg scryfallCard = FakeScryfallCardItemFactory.Create(new JObject { ["id"] = "test-id" });
        ScryfallCardItemToCardItemItrEntityMapper subject = new();

        // Act
        ICardItemItrEntity first = subject.Map(scryfallCard);
        ICardItemItrEntity second = subject.Map(scryfallCard);

        // Assert
        first.Should().NotBeSameAs(second);
    }
}
