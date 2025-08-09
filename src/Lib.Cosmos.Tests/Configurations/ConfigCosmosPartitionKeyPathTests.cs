using Lib.Cosmos.Configurations;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosPartitionKeyPathTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnConfigValue()
    {
        // Arrange
        string sourceKey = "test:partition:key:path";
        string expectedValue = "/partitionKey";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = expectedValue;
        ConfigCosmosPartitionKeyPath subject = new(sourceKey, fakeConfig);

        // Act
        string actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(expectedValue);
    }
}
