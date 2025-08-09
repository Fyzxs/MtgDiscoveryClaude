using Lib.Cosmos.Configurations;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosContainerTimeToLiveTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnParsedIntValue()
    {
        // Arrange
        string sourceKey = "test:ttl:seconds";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "3600";
        ConfigCosmosContainerTimeToLive subject = new(sourceKey, fakeConfig);

        // Act
        int? actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(3600);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldThrow_WhenValueIsNotNumeric()
    {
        // Arrange
        string sourceKey = "test:ttl:seconds";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "not-a-number";
        ConfigCosmosContainerTimeToLive subject = new(sourceKey, fakeConfig);

        // Act & assert
        ((System.Action)(() => subject.AsSystemType())).Should().Throw<CosmosConfigurationException>();
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldThrow_WhenValueIsEmpty()
    {
        // Arrange
        string sourceKey = "test:ttl:seconds";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "";
        ConfigCosmosContainerTimeToLive subject = new(sourceKey, fakeConfig);

        // Act & assert
        ((System.Action)(() => subject.AsSystemType())).Should().Throw<CosmosConfigurationException>();
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnNull_WhenValueIsNull()
    {
        // Arrange
        string sourceKey = "test:ttl:seconds";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = null;
        ConfigCosmosContainerTimeToLive subject = new(sourceKey, fakeConfig);

        // Act
        int? actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().BeNull();
    }
}
