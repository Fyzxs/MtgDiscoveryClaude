using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Configurations;
using Lib.Cosmos.Tests.Fakes;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosDatabaseConfigTests
{
    [TestMethod, TestCategory("unit")]
    public void ContainerConfig_ShouldReturnConfigCosmosContainerOptions()
    {
        // Arrange
        string parentKey = "test:database:key";
        string containerName = "TestContainer";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosDatabaseConfig subject = new(parentKey, fakeConfig);
        CosmosContainerDefinitionFake cosmosContainerDefinition = new()
        {
            ContainerNameResult = new CosmosContainerNameFake(containerName)
        };

        // Act
        ICosmosContainerConfig actual = subject.ContainerConfig(cosmosContainerDefinition);

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosContainerConfig>();
        classVariableTypeValidation.FieldShouldBeType<string>("_parentKey", $"{parentKey}:{containerName}").AssertFieldsAreExpectedType(actual);
    }

    [TestMethod, TestCategory("unit")]
    public void ContainerConfig_ShouldIncrementInvokeCount()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        ConfigCosmosDatabaseConfig subject = new("test:database:key", fakeConfig);
        CosmosContainerDefinitionFake cosmosContainerDefinition = new()
        {
            ContainerNameResult = new CosmosContainerNameFake("TestContainer")
        };

        // Act
        _ = subject.ContainerConfig(cosmosContainerDefinition);

        // Assert
        _ = cosmosContainerDefinition.ContainerNameInvokeCount.Should().Be(1);
    }
}
