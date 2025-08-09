using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Tests.Apis.Ids;

[TestClass]
public class PartitionCosmosPartitionKeyPathTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnSlashPartition()
    {
        // Arrange
        PartitionCosmosPartitionKeyPath subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be("/partition");
    }

    [TestMethod, TestCategory("unit")]
    public void ImplicitConversion_ShouldReturnSlashPartition()
    {
        // Arrange
        PartitionCosmosPartitionKeyPath subject = new();

        // Act
        string actual = subject;

        // Assert
        _ = actual.Should().Be("/partition");
    }
}