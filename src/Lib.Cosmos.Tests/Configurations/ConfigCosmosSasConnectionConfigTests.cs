using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Configurations;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosSasConnectionConfigTests
{
    [TestMethod, TestCategory("unit")]
    public void ConnectionString_ShouldReturnConfigCosmosConnectionString()
    {
        // Arrange
        string parentKey = "test:sas:connection";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosSasConnectionConfig subject = new(parentKey, fakeConfig);

        // Act
        CosmosConnectionString actual = subject.ConnectionString();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosConnectionString>();
        classVariableTypeValidation.FieldShouldBeType<string>("_sourceKey", $"{parentKey}:connectionstring").AssertFieldsAreExpectedType(actual);
    }

    [TestMethod, TestCategory("unit")]
    public void PreferredRegions_ShouldReturnConfigCosmosPreferredRegions()
    {
        // Arrange
        string parentKey = "test:sas:connection";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosSasConnectionConfig subject = new(parentKey, fakeConfig);

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
        string parentKey = "test:sas:connection";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosSasConnectionConfig subject = new(parentKey, fakeConfig);

        // Act
        CosmosConnectionMode actual = subject.ConnectionMode();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosConnectionMode>();
        classVariableTypeValidation.FieldShouldBeType<string>("_sourceKey", $"{parentKey}:connection_mode").AssertFieldsAreExpectedType(actual);
    }
}
