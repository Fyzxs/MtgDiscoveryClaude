using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Configurations;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosSasConfigTests
{
    [TestMethod, TestCategory("unit")]
    public void ConnectionConfig_ShouldReturnConfigCosmosSasConnectionConfig()
    {
        // Arrange
        string parentKey = "test:sas:key";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosSasConfig subject = new(parentKey, fakeConfig);

        // Act
        ICosmosSasConnectionConfig actual = subject.ConnectionConfig();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosSasConnectionConfig>();
        classVariableTypeValidation.FieldShouldBeType<string>("_parentKey", $"{parentKey}:connection").AssertFieldsAreExpectedType(actual);
    }
}
