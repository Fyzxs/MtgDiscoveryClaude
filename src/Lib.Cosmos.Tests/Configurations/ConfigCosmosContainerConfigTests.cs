using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Configurations;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosContainerConfigTests
{
    [TestMethod, TestCategory("unit")]
    public void AutoscaleMax_ShouldReturnConfigCosmosContainerAutoscaleMax()
    {
        // Arrange
        string parentKey = "test:container:options";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosContainerConfig subject = new(parentKey, fakeConfig);

        // Act
        CosmosContainerAutoscaleMax actual = subject.AutoscaleMax();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosContainerAutoscaleMax>();
        classVariableTypeValidation.FieldShouldBeType<string>("_sourceKey", $"{parentKey}:autoscale_max").AssertFieldsAreExpectedType(actual);
    }

    [TestMethod, TestCategory("unit")]
    public void TimeToLive_ShouldReturnConfigCosmosContainerTimeToLive()
    {
        // Arrange
        string parentKey = "test:container:options";
        IConfig fakeConfig = new ConfigFake();
        ClassVariableTypeValidation classVariableTypeValidation = new();
        ConfigCosmosContainerConfig subject = new(parentKey, fakeConfig);

        // Act
        CosmosContainerTimeToLive actual = subject.TimeToLive();

        // Assert
        _ = actual.Should().BeOfType<ConfigCosmosContainerTimeToLive>();
        classVariableTypeValidation.FieldShouldBeType<string>("_sourceKey", $"{parentKey}:time_to_live_seconds").AssertFieldsAreExpectedType(actual);
    }
}
