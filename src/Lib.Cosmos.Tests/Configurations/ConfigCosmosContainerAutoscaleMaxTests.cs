using System;
using Lib.Cosmos.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosContainerAutoscaleMaxTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnParsedIntValue()
    {
        // Arrange
        string sourceKey = "test:autoscale:max";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "4000";
        ConfigCosmosContainerAutoscaleMax subject = new(sourceKey, fakeConfig);

        // Act
        int actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(4000);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldThrowFormatException_WhenValueIsNotNumeric()
    {
        // Arrange
        string sourceKey = "test:autoscale:max";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "not-a-number";
        ConfigCosmosContainerAutoscaleMax subject = new(sourceKey, fakeConfig);

        // Act
        Action act = () => subject.AsSystemType();

        // Assert
        _ = act.Should().Throw<CosmosConfigurationException>();
    }
}
