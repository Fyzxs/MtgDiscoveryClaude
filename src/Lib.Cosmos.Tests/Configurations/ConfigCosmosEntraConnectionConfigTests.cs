using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Configurations;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosEntraConnectionConfigTests
{
    [TestMethod, TestCategory("unit")]
    public void AccountEndpoint_ShouldReturnConfigCosmosAccountEndpoint()
    {
        // Arrange
        string parentKey = "test:entra:connection";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosEntraConnectionConfig subject = new(parentKey, fakeConfig);

        // Act
        CosmosAccountEndpoint actual = subject.AccountEndpoint();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosAccountEndpoint>();
        classVariableTypeValidation.FieldShouldBeType<string>("_sourceKey", $"{parentKey}:account_endpoint").AssertFieldsAreExpectedType(actual);
    }

    [TestMethod, TestCategory("unit")]
    public void PreferredRegions_ShouldReturnConfigCosmosPreferredRegions()
    {
        // Arrange
        string parentKey = "test:entra:connection";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosEntraConnectionConfig subject = new(parentKey, fakeConfig);

        // Act
        CosmosPreferredRegions actual = subject.PreferredRegions();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosPreferredRegions>();
        classVariableTypeValidation.FieldShouldBeType<string>("_sourceKey", $"{parentKey}:preferred_regions").AssertFieldsAreExpectedType(actual);
    }

    [TestMethod, TestCategory("unit")]
    public void ConnectionMode_ShouldReturnConfigCosmosConnectionMode()
    {
        // Arrange
        string parentKey = "test:entra:connection";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosEntraConnectionConfig subject = new(parentKey, fakeConfig);

        // Act
        CosmosConnectionMode actual = subject.ConnectionMode();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosConnectionMode>();
        classVariableTypeValidation.FieldShouldBeType<string>("_sourceKey", $"{parentKey}:connection_mode").AssertFieldsAreExpectedType(actual);
    }
}
