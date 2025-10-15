using Lib.Cosmos.Apis;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Configurations;
using Lib.Cosmos.Tests.Fakes;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosConfigurationTests
{
    private sealed class InstanceWrapper : TypeWrapper<ConfigCosmosConfiguration>
    {
        public InstanceWrapper(IConfig config) : base(config) { }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_ShouldCreateInstanceWithMonoStateConfig()
    {
        // Arrange
        ClassVariableTypeValidation classVariableTypeValidation = new();

        // Act
        ConfigCosmosConfiguration actual = new();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosConfiguration>();
        classVariableTypeValidation.FieldShouldBeType<MonoStateConfig>("_config").AssertFieldsAreExpectedType(actual);
    }

    [TestMethod, TestCategory("unit")]
    public void AccountConfig_ShouldReturnConfigCosmosAccountConfiguration()
    {
        // Arrange
        string accountName = "TestAccount";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosConfiguration subject = new InstanceWrapper(fakeConfig);
        CosmosContainerDefinitionFake cosmosContainerDefinition = new()
        {
            FriendlyAccountNameResult = new CosmosFriendlyAccountNameFake(accountName)
        };

        // Act
        ICosmosAccountConfig actual = subject.AccountConfig(cosmosContainerDefinition);

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosAccountConfig>();
        classVariableTypeValidation.FieldShouldBeType<string>("_parentKey", $"{ICosmosConfiguration.CerberusCosmosConfigKey}:{accountName}").AssertFieldsAreExpectedType(actual);
    }

    [TestMethod, TestCategory("unit")]
    public void AccountConfig_ShouldIncrementInvokeCount()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        ConfigCosmosConfiguration subject = new InstanceWrapper(fakeConfig);
        CosmosContainerDefinitionFake cosmosContainerDefinition = new()
        {
            FriendlyAccountNameResult = new CosmosFriendlyAccountNameFake("TestFriendly")
        };

        // Act
        _ = subject.AccountConfig(cosmosContainerDefinition);

        // Assert
        _ = cosmosContainerDefinition.FriendlyAccountNameInvokeCount.Should().Be(1);
    }
}
