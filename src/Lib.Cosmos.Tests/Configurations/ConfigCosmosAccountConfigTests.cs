using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Configurations;
using Lib.Cosmos.Tests.Fakes;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosAccountConfigTests
{

    [TestMethod, TestCategory("unit")]
    public void AuthMode_ShouldReturnConfigCosmosClientAuthMode_WithCorrectParentKey()
    {
        // Arrange
        string parentKey = "test:parent:key";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosAccountConfig subject = new(parentKey, fakeConfig);

        // Act
        CosmosClientAuthMode actual = subject.AuthMode();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosClientAuthMode>();
        classVariableTypeValidation.FieldShouldBeType<string>("_sourceKey", $"{parentKey}:auth_mode").AssertFieldsAreExpectedType(actual);
    }

    [TestMethod, TestCategory("unit")]
    public void DatabaseConfig_ShouldReturnConfigCosmosDatabaseConfiguration_WithCorrectParentKey()
    {
        // Arrange
        string parentKey = "test:parent:key";
        string databaseName = "TestDatabase";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosAccountConfig subject = new(parentKey, fakeConfig);
        CosmosContainerDefinitionFake cosmosContainerDefinition = new()
        {
            DatabaseNameResult = new CosmosDatabaseNameFake(databaseName)
        };

        // Act
        ICosmosDatabaseConfig actual = subject.DatabaseConfig(cosmosContainerDefinition);

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosDatabaseConfig>();
        classVariableTypeValidation.FieldShouldBeType<string>("_parentKey", $"{parentKey}:{databaseName}").AssertFieldsAreExpectedType(actual);
    }

    [TestMethod, TestCategory("unit")]
    public void EntraConfig_ShouldReturnConfigCosmosEntraConfig()
    {
        // Arrange
        string parentKey = "test:parent:key";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosAccountConfig subject = new(parentKey, fakeConfig);

        // Act
        ICosmosEntraConfig actual = subject.EntraConfig();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosEntraConfig>();
        classVariableTypeValidation.FieldShouldBeType<string>("_parentKey", $"{parentKey}:entra").AssertFieldsAreExpectedType(actual);
    }

    [TestMethod, TestCategory("unit")]
    public void SasConfig_ShouldReturnConfigCosmosSasConfig()
    {
        // Arrange
        string parentKey = "test:parent:key";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosAccountConfig subject = new(parentKey, fakeConfig);

        // Act
        ICosmosSasConfig actual = subject.SasConfig();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosSasConfig>();
        classVariableTypeValidation.FieldShouldBeType<string>("_parentKey", $"{parentKey}:sas").AssertFieldsAreExpectedType(actual);
    }
}
