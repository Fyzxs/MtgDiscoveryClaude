using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosClientAuthModeTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnKeyAuth_WhenConfigValueIsKeyAuth()
    {
        // Arrange
        string sourceKey = "test:auth:mode";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "KeyAuth";
        ConfigCosmosClientAuthMode subject = new(sourceKey, fakeConfig);

        // Act
        CosmosAuthMode actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(CosmosAuthMode.KeyAuth);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnKeyAuth_WhenConfigValueIsKeyAuthCaseInsensitive()
    {
        // Arrange
        string sourceKey = "test:auth:mode";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "keyauth";
        ConfigCosmosClientAuthMode subject = new(sourceKey, fakeConfig);

        // Act
        CosmosAuthMode actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(CosmosAuthMode.KeyAuth);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnEntraAuth_WhenConfigValueIsNotKeyAuth()
    {
        // Arrange
        string sourceKey = "test:auth:mode";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "EntraAuth";
        ConfigCosmosClientAuthMode subject = new(sourceKey, fakeConfig);

        // Act
        CosmosAuthMode actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(CosmosAuthMode.EntraAuth);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnEntraAuth_WhenConfigValueIsEmpty()
    {
        // Arrange
        string sourceKey = "test:auth:mode";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "";
        ConfigCosmosClientAuthMode subject = new(sourceKey, fakeConfig);

        // Act
        CosmosAuthMode actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(CosmosAuthMode.EntraAuth);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnEntraAuth_WhenConfigValueIsNull()
    {
        // Arrange
        string sourceKey = "test:auth:mode";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = null;
        ConfigCosmosClientAuthMode subject = new(sourceKey, fakeConfig);

        // Act
        CosmosAuthMode actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(CosmosAuthMode.EntraAuth);
    }
}
