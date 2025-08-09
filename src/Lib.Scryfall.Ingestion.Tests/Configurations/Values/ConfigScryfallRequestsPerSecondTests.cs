using System;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Ingestion.Configurations.Values;
using TestConvenience.Core.Fakes;

namespace Lib.Scryfall.Ingestion.Tests.Configurations.Values;

[TestClass]
public sealed class ConfigScryfallRequestsPerSecondTests
{
    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_ValidRequestsPerSecond_ReturnsRequestsPerSecond()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "10" };
        ConfigScryfallRequestsPerSecond configValue = new(key, config);

        // Act
        int actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(10);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MinimumValue_ReturnsValue()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "1" };
        ConfigScryfallRequestsPerSecond configValue = new(key, config);

        // Act
        int actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(1);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MaximumValue_ReturnsValue()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "100" };
        ConfigScryfallRequestsPerSecond configValue = new(key, config);

        // Act
        int actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(100);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_NullValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = null! };
        ConfigScryfallRequestsPerSecond configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallRequestsPerSecond requires key [{key}]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_EmptyValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "" };
        ConfigScryfallRequestsPerSecond configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallRequestsPerSecond requires key [{key}]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_WhitespaceValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "   " };
        ConfigScryfallRequestsPerSecond configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallRequestsPerSecond requires key [{key}]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_InvalidFormat_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "not-a-number" };
        ConfigScryfallRequestsPerSecond configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallRequestsPerSecond Invalid value [{key}=not-a-number]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_BelowMinimum_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "0" };
        ConfigScryfallRequestsPerSecond configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallRequestsPerSecond value must be between 1 and 100 [{key}=0]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_AboveMaximum_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "101" };
        ConfigScryfallRequestsPerSecond configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallRequestsPerSecond value must be between 1 and 100 [{key}=101]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MissingKey_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new();
        ConfigScryfallRequestsPerSecond configValue = new($"{key}_missing", config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallRequestsPerSecond requires key [{key}_missing]");
    }
}
