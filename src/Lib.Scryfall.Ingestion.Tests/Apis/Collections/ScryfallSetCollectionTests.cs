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
public sealed class ScryfallSetCollectionTests
{
    [TestMethod]
    [TestCategory("unit")]
    public async Task GetAsyncEnumerator_ReturnsSetsFromPaging()
    {
        // Arrange
        dynamic rawData1 = JsonConvert.DeserializeObject(@"{
            'id': 'set1',
            'code': 'TST1',
            'name': 'Test Set 1',
            'search_uri': 'https://api.scryfall.com/cards/search?q=set:tst1'
        }");
        dynamic rawData2 = JsonConvert.DeserializeObject(@"{
            'id': 'set2',
            'code': 'TST2',
            'name': 'Test Set 2',
            'search_uri': 'https://api.scryfall.com/cards/search?q=set:tst2'
        }");

        List<ExtScryfallSetDto> sets =
        [
            new ExtScryfallSetDto(rawData1),
            new ExtScryfallSetDto(rawData2)
        ];

        ScryfallListPagingFake<ExtScryfallSetDto> pagingFake = new() { ItemsResult = sets };
        ScryfallSetCollection subject = new(pagingFake);
        List<IScryfallSet> actual = [];

        // Act
        await foreach (IScryfallSet set in subject.ConfigureAwait(false))
        {
            actual.Add(set);
        }

        // Assert
        _ = actual.Should().HaveCount(2);
        _ = actual[0].Name().Should().Be("Test Set 1");
        _ = actual[1].Name().Should().Be("Test Set 2");
        _ = pagingFake.ItemsInvokeCount.Should().Be(1);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task GetAsyncEnumerator_EmptyPaging_ReturnsNoItems()
    {
        // Arrange
        ScryfallListPagingFake<ExtScryfallSetDto> pagingFake = new() { ItemsResult = [] };
        ScryfallSetCollection subject = new(pagingFake);
        List<IScryfallSet> actual = [];

        // Act
        await foreach (IScryfallSet set in subject.ConfigureAwait(false))
        {
            actual.Add(set);
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
        dynamic rawData1 = JsonConvert.DeserializeObject(@"{
            'id': 'set1',
            'code': 'TST1',
            'name': 'Test Set 1',
            'search_uri': 'https://api.scryfall.com/cards/search?q=set:tst1'
        }");
        dynamic rawData2 = JsonConvert.DeserializeObject(@"{
            'id': 'set2',
            'code': 'TST2',
            'name': 'Test Set 2',
            'search_uri': 'https://api.scryfall.com/cards/search?q=set:tst2'
        }");

        List<ExtScryfallSetDto> sets =
        [
            new ExtScryfallSetDto(rawData1),
            new ExtScryfallSetDto(rawData2)
        ];

        ScryfallListPagingFake<ExtScryfallSetDto> pagingFake = new() { ItemsResult = sets };
        ScryfallSetCollection subject = new(pagingFake);
        using CancellationTokenSource cts = new();
        List<IScryfallSet> actual = [];

        // Act
        await foreach (IScryfallSet set in subject.WithCancellation(cts.Token).ConfigureAwait(false))
        {
            actual.Add(set);
            await cts.CancelAsync().ConfigureAwait(false);
            break;
        }

        // Assert
        _ = actual.Should().HaveCount(1);
        _ = actual[0].Name().Should().Be("Test Set 1");
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task GetAsyncEnumerator_ReturnsCorrectSetData()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'id': 'test-id',
            'code': 'TST',
            'name': 'Test Set',
            'search_uri': 'https://api.scryfall.com/cards/search?q=set:tst',
            'released_at': '2024-01-01',
            'set_type': 'expansion'
        }");
        ExtScryfallSetDto dto = new(rawData);
        ScryfallListPagingFake<ExtScryfallSetDto> pagingFake = new() { ItemsResult = [dto] };
        ScryfallSetCollection subject = new(pagingFake);
        List<IScryfallSet> actual = [];

        // Act
        await foreach (IScryfallSet set in subject.ConfigureAwait(false))
        {
            actual.Add(set);
        }

        // Assert
        _ = actual.Should().HaveCount(1);
        IScryfallSet actualSet = actual[0];
        _ = actualSet.Should().NotBeNull();
        _ = actualSet.Name().Should().Be("Test Set");
        _ = ((string)actualSet.Data().id).Should().Be("test-id");
        _ = ((string)actualSet.Data().code).Should().Be("TST");
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task GetAsyncEnumerator_MultipleCalls_CreatesNewEnumerator()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{
            'id': 'set1',
            'code': 'TST1',
            'name': 'Test Set 1',
            'search_uri': 'https://api.scryfall.com/cards/search?q=set:tst1'
        }");

        List<ExtScryfallSetDto> sets = [new ExtScryfallSetDto(rawData)];
        ScryfallListPagingFake<ExtScryfallSetDto> pagingFake = new() { ItemsResult = sets };
        ScryfallSetCollection subject = new(pagingFake);

        // Act
        int count1 = 0;
        await foreach (IScryfallSet _ in subject.ConfigureAwait(false))
        {
            count1++;
        }

        int count2 = 0;
        await foreach (IScryfallSet _ in subject.ConfigureAwait(false))
        {
            count2++;
        }

        // Assert
        _ = count1.Should().Be(1);
        _ = count2.Should().Be(1);
        _ = pagingFake.ItemsInvokeCount.Should().Be(2); // Called twice, once for each enumeration
    }
}
