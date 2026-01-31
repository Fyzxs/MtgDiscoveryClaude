using System;
using Lib.Cosmos.Configurations;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public sealed class ConfigCosmosDatabaseAutoscaleMaxTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnParsedIntValue()
    {
        // Arrange
        string sourceKey = "test:database:autoscale:max";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "4000";
        ConfigCosmosDatabaseAutoscaleMax subject = new(sourceKey, fakeConfig);

        // Act
        int actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(4000);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnMinimumValidValue()
    {
        // Arrange
        string sourceKey = "test:database:autoscale:max";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "1000";
        ConfigCosmosDatabaseAutoscaleMax subject = new(sourceKey, fakeConfig);

        // Act
        int actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(1000);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldThrowCosmosConfigurationException_WhenValueIsNotNumeric()
    {
        // Arrange
        string sourceKey = "test:database:autoscale:max";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "not-a-number";
        ConfigCosmosDatabaseAutoscaleMax subject = new(sourceKey, fakeConfig);

        // Act
        Action act = () => subject.AsSystemType();

        // Assert
        _ = act.Should().Throw<CosmosConfigurationException>();
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldThrowCosmosConfigurationException_WhenKeyIsMissing()
    {
        // Arrange
        string sourceKey = "test:database:autoscale:max";
        IConfig fakeConfig = new ConfigFake();
        ConfigCosmosDatabaseAutoscaleMax subject = new(sourceKey, fakeConfig);

        // Act
        Action act = () => subject.AsSystemType();

        // Assert
        _ = act.Should().Throw<CosmosConfigurationException>();
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldThrowCosmosConfigurationException_WhenValueIsEmpty()
    {
        // Arrange
        string sourceKey = "test:database:autoscale:max";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "";
        ConfigCosmosDatabaseAutoscaleMax subject = new(sourceKey, fakeConfig);

        // Act
        Action act = () => subject.AsSystemType();

        // Assert
        _ = act.Should().Throw<CosmosConfigurationException>();
    }
}
