using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Cosmos.Tests.Apis.Operators;

[TestClass]
public class PointItemTests
{
    [TestMethod, TestCategory("unit")]
    public void Id_ShouldBeInitializable()
    {
        // Arrange
        ProvidedCosmosItemId expected = new("test-id");

        // Act
        PointItem subject = new TestPointItem() { Id = expected };

        // Assert
        _ = subject.Id.Should().Be(expected);
    }

    [TestMethod, TestCategory("unit")]
    public void Partition_ShouldBeInitializable()
    {
        // Arrange
        ProvidedPartitionKeyValue expected = new("test-partition");

        // Act
        PointItem subject = new TestPointItem() { Partition = expected };

        // Assert
        _ = subject.Partition.Should().Be(expected);
    }

    [TestMethod, TestCategory("unit")]
    public void BothProperties_ShouldBeInitializable()
    {
        // Arrange
        ProvidedCosmosItemId expectedId = new("test-id");
        ProvidedPartitionKeyValue expectedPartition = new("test-partition");

        // Act
        PointItem subject = new TestPointItem()
        {
            Id = expectedId,
            Partition = expectedPartition
        };

        // Assert
        _ = subject.Id.Should().Be(expectedId);
        _ = subject.Partition.Should().Be(expectedPartition);
    }

    private sealed class TestPointItem : PointItem { }
}
