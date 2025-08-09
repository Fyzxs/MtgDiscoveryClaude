using Lib.Cosmos.Apis.Ids;

namespace Lib.Cosmos.Tests.Apis.Ids;

[TestClass]
public class ProvidedCosmosItemIdTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnProvidedValue()
    {
        // Arrange
        const string Expected = "test-item-id";
        ProvidedCosmosItemId subject = new(Expected);

        // Act
        string actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(Expected);
    }

    [TestMethod, TestCategory("unit")]
    public void ImplicitConversion_ShouldReturnProvidedValue()
    {
        // Arrange
        const string Expected = "test-item-id";
        ProvidedCosmosItemId subject = new(Expected);

        // Act
        string actual = subject;

        // Assert
        _ = actual.Should().Be(Expected);
    }
}