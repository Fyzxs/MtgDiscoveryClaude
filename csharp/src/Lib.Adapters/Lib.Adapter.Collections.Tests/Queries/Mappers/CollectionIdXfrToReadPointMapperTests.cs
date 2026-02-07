using System.Threading.Tasks;
using Lib.Adapter.Collections.Queries.Mappers;
using Lib.Adapter.Collections.Tests.Fakes;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Collections.Tests.Queries.Mappers;

[TestClass]
public sealed class CollectionIdXfrToReadPointMapperTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Map_WithCollectionIdAndOwnerId_ReturnsReadPointItem()
    {
        // Arrange
        const string expectedCollectionId = "col-123";
        const string expectedOwnerId = "owner-456";
        CollectionIdXfrEntityFake input = new()
        {
            CollectionId = expectedCollectionId,
            OwnerId = expectedOwnerId
        };
        CollectionIdXfrToReadPointMapper subject = new();

        // Act
        ReadPointItem actual = await subject.Map(input).ConfigureAwait(false);

        // Assert
        actual.Id.AsSystemType().Should().Be(expectedCollectionId);
        actual.Partition.AsSystemType().ToString().Should().Be($"[\"{expectedOwnerId}\"]");
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_WithEmptyValues_ReturnsReadPointItemWithEmptyValues()
    {
        // Arrange
        CollectionIdXfrEntityFake input = new()
        {
            CollectionId = string.Empty,
            OwnerId = string.Empty
        };
        CollectionIdXfrToReadPointMapper subject = new();

        // Act
        ReadPointItem actual = await subject.Map(input).ConfigureAwait(false);

        // Assert
        actual.Id.AsSystemType().Should().BeEmpty();
        actual.Partition.AsSystemType().ToString().Should().Be("[\"\"]");
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_ReturnsNewInstanceEachTime()
    {
        // Arrange
        CollectionIdXfrEntityFake input = new()
        {
            CollectionId = "col-123",
            OwnerId = "owner-456"
        };
        CollectionIdXfrToReadPointMapper subject = new();

        // Act
        ReadPointItem firstCall = await subject.Map(input).ConfigureAwait(false);
        ReadPointItem secondCall = await subject.Map(input).ConfigureAwait(false);

        // Assert
        firstCall.Should().NotBeSameAs(secondCall);
    }
}
