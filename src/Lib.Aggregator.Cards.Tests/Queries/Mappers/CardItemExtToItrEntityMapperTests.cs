using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Aggregator.Cards.Tests.Fakes;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;
using Newtonsoft.Json.Linq;

namespace Lib.Aggregator.Cards.Tests.Queries.Mappers;

[TestClass]
public sealed class CardItemExtToItrMapperTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Map_WithNullInput_ReturnsNull()
    {
        // Arrange
        CardItemExtToItrMapper subject = new();

        // Act
        ICardItemItrEntity actual = await subject.Map(null).ConfigureAwait(false);

        // Assert
        actual.Should().BeNull();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_WithValidScryfallCard_ReturnsCardItemItrEntity()
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

        ScryfallCardItemExtEntity scryfallCard = ScryfallCardItemFactoryFake.Create(testData);
        CardItemExtToItrMapper subject = new();

        // Act
        ICardItemItrEntity actual = await subject.Map(scryfallCard).ConfigureAwait(false);

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
    public async Task Map_WithEmptyData_ReturnsEntityWithNullProperties()
    {
        // Arrange
        ScryfallCardItemExtEntity scryfallCard = ScryfallCardItemFactoryFake.Create(new JObject());
        CardItemExtToItrMapper subject = new();

        // Act
        ICardItemItrEntity actual = await subject.Map(scryfallCard).ConfigureAwait(false);

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
    public async Task Map_ReturnsNewInstanceEachTime()
    {
        // Arrange
        ScryfallCardItemExtEntity scryfallCard = ScryfallCardItemFactoryFake.Create(new JObject { ["id"] = "test-id" });
        CardItemExtToItrMapper subject = new();

        // Act
        ICardItemItrEntity first = await subject.Map(scryfallCard).ConfigureAwait(false);
        ICardItemItrEntity second = await subject.Map(scryfallCard).ConfigureAwait(false);

        // Assert
        first.Should().NotBeSameAs(second);
    }
}
