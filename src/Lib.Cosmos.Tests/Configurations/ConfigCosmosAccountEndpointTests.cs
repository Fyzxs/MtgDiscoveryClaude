using Lib.Cosmos.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosAccountEndpointTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnConfigValue()
    {
        // Arrange
        string sourceKey = "test:endpoint:key";
        string expectedValue = "https://test.documents.azure.com:443/";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = expectedValue;
        ConfigCosmosAccountEndpoint subject = new(sourceKey, fakeConfig);

        // Act
        string actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(expectedValue);
    }
}
