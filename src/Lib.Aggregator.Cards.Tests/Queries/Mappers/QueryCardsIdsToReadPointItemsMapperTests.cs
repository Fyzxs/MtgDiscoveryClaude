using System.Linq;
using System.Threading.Tasks;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Aggregator.Cards.Tests.Fakes;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Aggregator.Cards.Tests.Queries.Mappers;

[TestClass]
public sealed class QueryCardsIdsToReadPointItemsMapperTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Map_WithEmptyCardIds_ReturnsEmptyCollection()
    {
        // Arrange
        CardIdsItrEntityFake args = new()
        {
            CardIds = []
        };
        QueryCardsIdsToReadPointItemsMapper subject = new();

        // Act
        ReadPointItem[] actual = [.. await subject.Map(args).ConfigureAwait(false)];

        // Assert
        actual.Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_WithSingleCardId_ReturnsSingleReadPointItem()
    {
        // Arrange
        const string expectedId = "test-card-id";
        CardIdsItrEntityFake args = new()
        {
            CardIds = [expectedId]
        };
        QueryCardsIdsToReadPointItemsMapper subject = new();

        // Act
        ReadPointItem[] actual = [.. await subject.Map(args).ConfigureAwait(false)];

        // Assert
        actual.Should().HaveCount(1);
        actual[0].Id.AsSystemType().Should().Be(expectedId);
        actual[0].Partition.AsSystemType().ToString().Should().Be($"[\"{expectedId}\"]");
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_WithMultipleCardIds_ReturnsMultipleReadPointItems()
    {
        // Arrange
        string[] expectedIds = ["card-id-1", "card-id-2", "card-id-3"];
        CardIdsItrEntityFake args = new()
        {
            CardIds = expectedIds
        };
        QueryCardsIdsToReadPointItemsMapper subject = new();

        // Act
        ReadPointItem[] actual = [.. await subject.Map(args).ConfigureAwait(false)];

        // Assert
        actual.Should().HaveCount(3);
        for (int i = 0; i < expectedIds.Length; i++)
        {
            actual[i].Id.AsSystemType().Should().Be(expectedIds[i]);
            actual[i].Partition.AsSystemType().ToString().Should().Be($"[\"{expectedIds[i]}\"]");
        }
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_ReturnsNewInstancesEachTime()
    {
        // Arrange
        CardIdsItrEntityFake args = new()
        {
            CardIds = ["card-id"]
        };
        QueryCardsIdsToReadPointItemsMapper subject = new();

        // Act
        ReadPointItem[] firstCall = [.. await subject.Map(args).ConfigureAwait(false)];
        ReadPointItem[] secondCall = [.. await subject.Map(args).ConfigureAwait(false)];

        // Assert
        firstCall.Should().NotBeSameAs(secondCall);
        firstCall[0].Should().NotBeSameAs(secondCall[0]);
    }
}
