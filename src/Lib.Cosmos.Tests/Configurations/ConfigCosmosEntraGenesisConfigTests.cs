using Lib.Cosmos.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosEntraGenesisConfigTests
{
    [TestMethod, TestCategory("unit")]
    public void Bypass_ShouldReturnTrue_WhenConfigValueIsTrue()
    {
        // Arrange
        string parentKey = "test:entra:genesis";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[$"{parentKey}:bypass"] = "true";
        ConfigCosmosEntraGenesisConfig subject = new(parentKey, fakeConfig);

        // Act
        bool actual = subject.Bypass();

        // Assert
        _ = actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void Bypass_ShouldReturnFalse_WhenConfigValueIsFalse()
    {
        // Arrange
        string parentKey = "test:entra:genesis";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[$"{parentKey}:bypass"] = "false";
        ConfigCosmosEntraGenesisConfig subject = new(parentKey, fakeConfig);

        // Act
        bool actual = subject.Bypass();

        // Assert
        _ = actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Bypass_ShouldReturnFalse_WhenConfigValueIsNotParseable()
    {
        // Arrange
        string parentKey = "test:entra:genesis";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[$"{parentKey}:bypass"] = "not-a-bool";
        ConfigCosmosEntraGenesisConfig subject = new(parentKey, fakeConfig);

        // Act
        bool actual = subject.Bypass();

        // Assert
        _ = actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void SubscriptionId_ShouldReturnConfigValue()
    {
        // Arrange
        string parentKey = "test:entra:genesis";
        string expectedValue = "12345678-1234-1234-1234-123456789012";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[$"{parentKey}:subscription_id"] = expectedValue;
        ConfigCosmosEntraGenesisConfig subject = new(parentKey, fakeConfig);

        // Act
        string actual = subject.SubscriptionId();

        // Assert
        _ = actual.Should().Be(expectedValue);
    }

    [TestMethod, TestCategory("unit")]
    public void ResourceGroupName_ShouldReturnConfigValue()
    {
        // Arrange
        string parentKey = "test:entra:genesis";
        string expectedValue = "test-resource-group";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[$"{parentKey}:resource_group_name"] = expectedValue;
        ConfigCosmosEntraGenesisConfig subject = new(parentKey, fakeConfig);

        // Act
        string actual = subject.ResourceGroupName();

        // Assert
        _ = actual.Should().Be(expectedValue);
    }
}
