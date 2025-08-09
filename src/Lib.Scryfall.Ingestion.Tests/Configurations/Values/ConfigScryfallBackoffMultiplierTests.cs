using System;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Ingestion.Configurations.Values;
using TestConvenience.Core.Fakes;

namespace Lib.Scryfall.Ingestion.Tests.Configurations.Values;

[TestClass]
public sealed class ConfigScryfallBackoffMultiplierTests
{
    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_ValidMultiplier_ReturnsMultiplier()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "2.5" };
        ConfigScryfallBackoffMultiplier configValue = new(key, config);

        // Act
        double actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(2.5);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MinimumValue_ReturnsValue()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "1.0" };
        ConfigScryfallBackoffMultiplier configValue = new(key, config);

        // Act
        double actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(1.0);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MaximumValue_ReturnsValue()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "10.0" };
        ConfigScryfallBackoffMultiplier configValue = new(key, config);

        // Act
        double actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(10.0);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_NullValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = null! };
        ConfigScryfallBackoffMultiplier configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallBackoffMultiplier Invalid value [{key}=]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_EmptyValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "" };
        ConfigScryfallBackoffMultiplier configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallBackoffMultiplier Invalid value [{key}=]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_WhitespaceValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = " \t\r\n " };
        ConfigScryfallBackoffMultiplier configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallBackoffMultiplier Invalid value [{key}= \t\r\n ]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_InvalidFormat_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "not-a-number" };
        ConfigScryfallBackoffMultiplier configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallBackoffMultiplier Invalid value [{key}=not-a-number]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_BelowMinimum_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "0.9" };
        ConfigScryfallBackoffMultiplier configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallBackoffMultiplier value must be between 1 and 10 [{key}=0.9]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_AboveMaximum_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "10.1" };
        ConfigScryfallBackoffMultiplier configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallBackoffMultiplier value must be between 1 and 10 [{key}=10.1]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MissingKey_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new();
        ConfigScryfallBackoffMultiplier configValue = new($"{key}_missing", config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallBackoffMultiplier Invalid value [{key}_missing=]");
    }
}
