using System;
using Lib.Cosmos.Configurations;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosConnectionStringTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnConfigValue()
    {
        // Arrange
        string sourceKey = "test:connection:string";
        string expectedValue = "AccountEndpoint=https://blackhole.localhost:443/;AccountKey=testkey==;";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = expectedValue;
        ConfigCosmosConnectionString subject = new(sourceKey, fakeConfig);

        // Act
        string actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(expectedValue);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnEmptyString_WhenConfigValueIsEmpty()
    {
        // Arrange
        string sourceKey = "test:connection:string";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "";
        ConfigCosmosConnectionString subject = new(sourceKey, fakeConfig);

        // Act
        Action act = () => subject.AsSystemType();

        // Assert
        act.Should().Throw<CosmosConfigurationException>().WithMessage("ConfigCosmosConnectionString requires key [test:connection:string]");
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnNull_WhenConfigValueIsNull()
    {
        // Arrange
        string sourceKey = "test:connection:string";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = null;
        ConfigCosmosConnectionString subject = new(sourceKey, fakeConfig);

        // Act
        Action act = () => subject.AsSystemType();

        // Assert
        act.Should().Throw<CosmosConfigurationException>().WithMessage("ConfigCosmosConnectionString requires key [test:connection:string]");
    }
}
