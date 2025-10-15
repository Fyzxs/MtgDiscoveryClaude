using System;
using Lib.Cosmos.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosConnectionModeTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnGateway_WhenConfigValueIsGateway()
    {
        // Arrange
        string sourceKey = "test:connection:mode";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "gateway";
        ConfigCosmosConnectionMode subject = new(sourceKey, fakeConfig);

        // Act
        ConnectionMode actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(ConnectionMode.Gateway);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnGateway_WhenConfigValueIsGatewayCaseInsensitive()
    {
        // Arrange
        string sourceKey = "test:connection:mode";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "GATEWAY";
        ConfigCosmosConnectionMode subject = new(sourceKey, fakeConfig);

        // Act
        ConnectionMode actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(ConnectionMode.Gateway);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnDirect_WhenConfigValueIsNotGateway()
    {
        // Arrange
        string sourceKey = "test:connection:mode";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "direct";
        ConfigCosmosConnectionMode subject = new(sourceKey, fakeConfig);

        // Act
        ConnectionMode actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(ConnectionMode.Direct);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldThrow_WhenConfigValueIsEmpty()
    {
        // Arrange
        string sourceKey = "test:connection:mode";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "";
        ConfigCosmosConnectionMode subject = new(sourceKey, fakeConfig);

        // Act
        Action act = () => subject.AsSystemType();

        // Assert
        act.Should().Throw<CosmosConfigurationException>().WithMessage("ConfigCosmosConnectionMode requires key [test:connection:mode]");

    }
}
