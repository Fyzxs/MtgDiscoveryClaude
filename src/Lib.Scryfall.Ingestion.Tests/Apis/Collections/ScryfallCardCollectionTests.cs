using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Tests.Fakes;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.Tests.Apis.Collections;

[TestClass]
public sealed class ScryfallCardCollectionTests
{
    [TestMethod]
    [TestCategory("unit")]
    public async Task GetAsyncEnumerator_ReturnsCardsFromPaging()
    {
        // Arrange
        dynamic rawSetData = JsonConvert.DeserializeObject(@"{
            'id': 'set1',
            'code': 'TST',
            'name': 'Test Set'
        }");
        IScryfallSet set = new ScryfallSetFake { Data = rawSetData };

        dynamic rawCard1 = JsonConvert.DeserializeObject(@"{
            'id': 'card1',
            'name': 'Lightning Bolt',
            'mana_cost': '{R}',
            'type_line': 'Instant',
            'oracle_text': 'Lightning Bolt deals 3 damage to any target.'
        }");
        dynamic rawCard2 = JsonConvert.DeserializeObject(@"{
            'id': 'card2',
            'name': 'Counterspell',
            'mana_cost': '{U}{U}',
            'type_line': 'Instant',
            'oracle_text': 'Counter target spell.'
        }");

        List<ExtScryfallCardDto> cards =
        [
            new ExtScryfallCardDto(rawCard1),
            new ExtScryfallCardDto(rawCard2)
        ];

        ScryfallListPagingFake<ExtScryfallCardDto> pagingFake = new() { ItemsResult = cards };
        ScryfallCardCollection subject = new(set, pagingFake);
        List<IScryfallCard> actual = [];

        // Act
        await foreach (IScryfallCard card in subject.ConfigureAwait(false))
        {
            actual.Add(card);
        }

        // Assert
        _ = actual.Should().HaveCount(2);
        _ = actual[0].Name().Should().Be("Lightning Bolt");
        _ = actual[1].Name().Should().Be("Counterspell");
        _ = pagingFake.ItemsInvokeCount.Should().Be(1);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task GetAsyncEnumerator_EmptyPaging_ReturnsNoItems()
    {
        // Arrange
        dynamic rawSetData = JsonConvert.DeserializeObject(@"{
            'id': 'set1',
            'code': 'TST',
            'name': 'Test Set'
        }");
        IScryfallSet set = new ScryfallSetFake { Data = rawSetData };
        ScryfallListPagingFake<ExtScryfallCardDto> pagingFake = new() { ItemsResult = [] };
        ScryfallCardCollection subject = new(set, pagingFake);
        List<IScryfallCard> actual = [];

        // Act
        await foreach (IScryfallCard card in subject.ConfigureAwait(false))
        {
            actual.Add(card);
        }

        // Assert
        _ = actual.Should().BeEmpty();
        _ = pagingFake.ItemsInvokeCount.Should().Be(1);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task GetAsyncEnumerator_WithCancellation_StopsIteration()
    {
        // Arrange
        dynamic rawSetData = JsonConvert.DeserializeObject(@"{
            'id': 'set1',
            'code': 'TST',
            'name': 'Test Set'
        }");
        IScryfallSet set = new ScryfallSetFake { Data = rawSetData };

        dynamic rawCard1 = JsonConvert.DeserializeObject(@"{
            'id': 'card1',
            'name': 'Lightning Bolt',
            'mana_cost': '{R}'
        }");
        dynamic rawCard2 = JsonConvert.DeserializeObject(@"{
            'id': 'card2',
            'name': 'Counterspell',
            'mana_cost': '{U}{U}'
        }");

        List<ExtScryfallCardDto> cards =
        [
            new ExtScryfallCardDto(rawCard1),
            new ExtScryfallCardDto(rawCard2)
        ];

        ScryfallListPagingFake<ExtScryfallCardDto> pagingFake = new() { ItemsResult = cards };
        ScryfallCardCollection subject = new(set, pagingFake);
        using CancellationTokenSource cts = new();
        List<IScryfallCard> actual = [];

        // Act
        await foreach (IScryfallCard card in subject.WithCancellation(cts.Token).ConfigureAwait(false))
        {
            actual.Add(card);
            await cts.CancelAsync().ConfigureAwait(false);
            break;
        }

        // Assert
        _ = actual.Should().HaveCount(1);
        _ = actual[0].Name().Should().Be("Lightning Bolt");
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task GetAsyncEnumerator_ReturnsCorrectCardData()
    {
        // Arrange
        dynamic rawSetData = JsonConvert.DeserializeObject(@"{
            'id': 'set1',
            'code': 'TST',
            'name': 'Test Set'
        }");
        IScryfallSet set = new ScryfallSetFake { Data = rawSetData };

        dynamic rawCard = JsonConvert.DeserializeObject(@"{
            'id': 'test-card-id',
            'name': 'Test Card',
            'mana_cost': '{2}{U}',
            'type_line': 'Creature — Test',
            'oracle_text': 'Flying\nWhen Test Card enters the battlefield, draw a card.',
            'power': '2',
            'toughness': '3',
            'rarity': 'rare'
        }");
        ExtScryfallCardDto dto = new(rawCard);
        ScryfallListPagingFake<ExtScryfallCardDto> pagingFake = new() { ItemsResult = [dto] };
        ScryfallCardCollection subject = new(set, pagingFake);
        List<IScryfallCard> actual = [];

        // Act
        await foreach (IScryfallCard card in subject.ConfigureAwait(false))
        {
            actual.Add(card);
        }

        // Assert
        _ = actual.Should().HaveCount(1);
        IScryfallCard actualCard = actual[0];
        _ = actualCard.Should().NotBeNull();
        _ = actualCard.Name().Should().Be("Test Card");
        _ = ((string)actualCard.Data().id).Should().Be("test-card-id");
        _ = ((string)actualCard.Data().mana_cost).Should().Be("{2}{U}");
        _ = ((string)actualCard.Data().type_line).Should().Be("Creature — Test");
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task GetAsyncEnumerator_MultipleCalls_CreatesNewEnumerator()
    {
        // Arrange
        dynamic rawSetData = JsonConvert.DeserializeObject(@"{
            'id': 'set1',
            'code': 'TST',
            'name': 'Test Set'
        }");
        IScryfallSet set = new ScryfallSetFake { Data = rawSetData };

        dynamic rawCard = JsonConvert.DeserializeObject(@"{
            'id': 'card1',
            'name': 'Test Card'
        }");

        List<ExtScryfallCardDto> cards = [new ExtScryfallCardDto(rawCard)];
        ScryfallListPagingFake<ExtScryfallCardDto> pagingFake = new() { ItemsResult = cards };
        ScryfallCardCollection subject = new(set, pagingFake);

        // Act
        int count1 = 0;
        await foreach (IScryfallCard _ in subject.ConfigureAwait(false))
        {
            count1++;
        }

        int count2 = 0;
        await foreach (IScryfallCard _ in subject.ConfigureAwait(false))
        {
            count2++;
        }

        // Assert
        _ = count1.Should().Be(1);
        _ = count2.Should().Be(1);
        _ = pagingFake.ItemsInvokeCount.Should().Be(2); // Called twice, once for each enumeration
    }
}
