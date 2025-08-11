using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Tests.Fakes;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.Tests.Apis.Collections;

[TestClass]
public sealed class HttpScryfallCollectionTests
{
    [TestMethod]
    [TestCategory("unit")]
    public async Task Items_TransformsAndReturnsAllItems()
    {
        // Arrange
        dynamic rawData1 = JsonConvert.DeserializeObject(@"{'id': '1', 'value': 'first'}");
        dynamic rawData2 = JsonConvert.DeserializeObject(@"{'id': '2', 'value': 'second'}");
        dynamic rawData3 = JsonConvert.DeserializeObject(@"{'id': '3', 'value': 'third'}");

        List<TestDto> dtos =
        [
            new TestDto(rawData1),
            new TestDto(rawData2),
            new TestDto(rawData3)
        ];

        ScryfallListPagingFake<TestDto> pagingFake = new() { ItemsResult = dtos };
        TestDtoTransformerFake<TestDto, TestDomainModel> transformerFake = new() { TransformResult = null };
        HttpScryfallCollection<TestDto, TestDomainModel> subject = new(pagingFake, transformerFake);
        List<TestDomainModel> actual = [];

        // Act
        await foreach (TestDomainModel item in subject.Items().ConfigureAwait(false))
        {
            actual.Add(item);
        }

        // Assert
        _ = actual.Should().HaveCount(3);
        _ = pagingFake.ItemsInvokeCount.Should().Be(1);
        _ = transformerFake.TransformInvokeCount.Should().Be(3);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task Items_EmptyPaging_ReturnsNoItems()
    {
        // Arrange
        ScryfallListPagingFake<TestDto> pagingFake = new() { ItemsResult = [] };
        TestDtoTransformerFake<TestDto, TestDomainModel> transformerFake = new() { TransformResult = null };
        HttpScryfallCollection<TestDto, TestDomainModel> subject = new(pagingFake, transformerFake);
        List<TestDomainModel> actual = [];

        // Act
        await foreach (TestDomainModel item in subject.Items().ConfigureAwait(false))
        {
            actual.Add(item);
        }

        // Assert
        _ = actual.Should().BeEmpty();
        _ = pagingFake.ItemsInvokeCount.Should().Be(1);
        _ = transformerFake.TransformInvokeCount.Should().Be(0);
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task Items_MultipleEnumerations_WorksIndependently()
    {
        // Arrange
        dynamic rawData = JsonConvert.DeserializeObject(@"{'id': '1', 'value': 'test'}");
        List<TestDto> dtos = [new TestDto(rawData)];

        ScryfallListPagingFake<TestDto> pagingFake = new() { ItemsResult = dtos };
        TestDtoTransformerFake<TestDto, TestDomainModel> transformerFake = new() { TransformResult = null };
        HttpScryfallCollection<TestDto, TestDomainModel> subject = new(pagingFake, transformerFake);

        // Act
        int count1 = 0;
        await foreach (TestDomainModel _ in subject.Items().ConfigureAwait(false))
        {
            count1++;
        }

        int count2 = 0;
        await foreach (TestDomainModel _ in subject.Items().ConfigureAwait(false))
        {
            count2++;
        }

        // Assert
        _ = count1.Should().Be(1);
        _ = count2.Should().Be(1);
        _ = pagingFake.ItemsInvokeCount.Should().Be(2);
        _ = transformerFake.TransformInvokeCount.Should().Be(2);
    }

    // Test DTOs and models
    private sealed class TestDto : IScryfallDto
    {
        private readonly dynamic _data;

        public TestDto(dynamic data)
        {
            _data = data;
        }

        public dynamic Data => _data;
    }

    private sealed class TestDomainModel
    {
        public string Value { get; init; }
    }
}
