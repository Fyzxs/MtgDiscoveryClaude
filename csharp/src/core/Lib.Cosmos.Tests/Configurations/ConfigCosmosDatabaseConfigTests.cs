using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Configurations;
using Lib.Cosmos.Tests.Fakes;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public sealed class ConfigCosmosDatabaseConfigTests
{
    [TestMethod, TestCategory("unit")]
    public void ContainerConfig_ShouldReturnConfigCosmosContainerConfig()
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

    [TestMethod, TestCategory("unit")]
    public void ThroughputMode_ShouldReturnConfigCosmosThroughputMode()
    {
        // Arrange
        string parentKey = "test:database:key";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosDatabaseConfig subject = new(parentKey, fakeConfig);

        // Act
        ICosmosThroughputMode actual = subject.ThroughputMode();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosThroughputMode>();
        classVariableTypeValidation.FieldShouldBeType<string>("_sourceKey", $"{parentKey}:{ICosmosDatabaseConfig.ThroughputModeKey}").AssertFieldsAreExpectedType(actual);
    }

    [TestMethod, TestCategory("unit")]
    public void AutoscaleMax_ShouldReturnConfigCosmosDatabaseAutoscaleMax()
    {
        // Arrange
        string parentKey = "test:database:key";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosDatabaseConfig subject = new(parentKey, fakeConfig);

        // Act
        CosmosAutoscaleMax actual = subject.AutoscaleMax();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosDatabaseAutoscaleMax>();
        classVariableTypeValidation.FieldShouldBeType<string>("_sourceKey", $"{parentKey}:{ICosmosDatabaseConfig.AutoscaleMaxKey}").AssertFieldsAreExpectedType(actual);
    }
}
