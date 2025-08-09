using System;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Ingestion.Configurations.Values;
using TestConvenience.Core.Fakes;

namespace Lib.Scryfall.Ingestion.Tests.Configurations.Values;

[TestClass]
public sealed class ConfigScryfallThrottleWarningThresholdTests
{
    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_ValidThreshold_ReturnsThreshold()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "0.8" };
        ConfigScryfallThrottleWarningThreshold configValue = new(key, config);

        // Act
        double actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(0.8);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_JustAboveMinimum_ReturnsValue()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "0.01" };
        ConfigScryfallThrottleWarningThreshold configValue = new(key, config);

        // Act
        double actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(0.01);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MaximumValue_ReturnsValue()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "1.0" };
        ConfigScryfallThrottleWarningThreshold configValue = new(key, config);

        // Act
        double actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(1.0);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_NullValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = null! };
        ConfigScryfallThrottleWarningThreshold configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallThrottleWarningThreshold requires key [{key}]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_EmptyValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "" };
        ConfigScryfallThrottleWarningThreshold configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallThrottleWarningThreshold requires key [{key}]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_WhitespaceValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "   " };
        ConfigScryfallThrottleWarningThreshold configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallThrottleWarningThreshold requires key [{key}]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_InvalidFormat_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "not-a-number" };
        ConfigScryfallThrottleWarningThreshold configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallThrottleWarningThreshold Invalid value [{key}=not-a-number]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_AtMinimum_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "0.0" };
        ConfigScryfallThrottleWarningThreshold configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallThrottleWarningThreshold value must be between 0 and 1 [{key}=0.0]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_BelowMinimum_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "-0.1" };
        ConfigScryfallThrottleWarningThreshold configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallThrottleWarningThreshold value must be between 0 and 1 [{key}=-0.1]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_AboveMaximum_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "1.1" };
        ConfigScryfallThrottleWarningThreshold configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallThrottleWarningThreshold value must be between 0 and 1 [{key}=1.1]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MissingKey_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new();
        ConfigScryfallThrottleWarningThreshold configValue = new($"{key}_missing", config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallThrottleWarningThreshold requires key [{key}_missing]");
    }
}
