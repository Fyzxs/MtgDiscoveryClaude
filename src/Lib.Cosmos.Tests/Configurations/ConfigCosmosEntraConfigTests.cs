using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Configurations;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosEntraConfigTests
{
    [TestMethod, TestCategory("unit")]
    public void ConnectionConfig_ShouldReturnConfigCosmosEntraConnectionConfig()
    {
        // Arrange
        string parentKey = "test:entra:key";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosEntraConfig subject = new(parentKey, fakeConfig);

        // Act
        ICosmosEntraConnectionConfig actual = subject.ConnectionConfig();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosEntraConnectionConfig>();
        classVariableTypeValidation.FieldShouldBeType<string>("_parentKey", $"{parentKey}:connection").AssertFieldsAreExpectedType(actual);
    }

    [TestMethod, TestCategory("unit")]
    public void GenesisConfig_ShouldReturnConfigCosmosEntraGenesisConfig()
    {
        // Arrange
        string parentKey = "test:entra:key";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosEntraConfig subject = new(parentKey, fakeConfig);

        // Act
        ICosmosEntraGenesisConfig actual = subject.GenesisConfig();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosEntraGenesisConfig>();
        classVariableTypeValidation.FieldShouldBeType<string>("_parentKey", $"{parentKey}:genesis").AssertFieldsAreExpectedType(actual);
    }
}
