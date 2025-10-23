using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Tests.Apis.Ids;

[TestClass]
public class ProvidedPartitionKeyValueTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnPartitionKeyWithProvidedValue()
    {
        // Arrange
        const string Value = "test-partition-key";
        ProvidedPartitionKeyValue subject = new(Value);

        // Act
        PartitionKey actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(new PartitionKey(Value));
    }

    [TestMethod, TestCategory("unit")]
    public void ImplicitConversion_ShouldReturnPartitionKeyWithProvidedValue()
    {
        // Arrange
        const string Value = "test-partition-key";
        ProvidedPartitionKeyValue subject = new(Value);

        // Act
        PartitionKey actual = subject;

        // Assert
        _ = actual.Should().Be(new PartitionKey(Value));
    }
}
