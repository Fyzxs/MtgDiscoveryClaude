using System;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Ingestion.Configurations.Values;
using TestConvenience.Core.Fakes;

namespace Lib.Scryfall.Ingestion.Tests.Configurations.Values;

[TestClass]
public sealed class ConfigScryfallMaxRetriesTests
{
    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_ValidMaxRetries_ReturnsMaxRetries()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "3" };
        ConfigScryfallMaxRetries configValue = new(key, config);

        // Act
        int actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(3);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MinimumValue_ReturnsValue()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "0" };
        ConfigScryfallMaxRetries configValue = new(key, config);

        // Act
        int actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(0);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MaximumValue_ReturnsValue()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "10" };
        ConfigScryfallMaxRetries configValue = new(key, config);

        // Act
        int actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(10);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_NullValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = null! };
        ConfigScryfallMaxRetries configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxRetries requires key [{key}]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_EmptyValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "" };
        ConfigScryfallMaxRetries configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxRetries requires key [{key}]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_WhitespaceValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "   " };
        ConfigScryfallMaxRetries configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxRetries requires key [{key}]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_InvalidFormat_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "not-a-number" };
        ConfigScryfallMaxRetries configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxRetries Invalid value [{key}=not-a-number]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_BelowMinimum_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "-1" };
        ConfigScryfallMaxRetries configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxRetries value must be between 0 and 10 [{key}=-1]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_AboveMaximum_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "11" };
        ConfigScryfallMaxRetries configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxRetries value must be between 0 and 10 [{key}=11]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MissingKey_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new();
        ConfigScryfallMaxRetries configValue = new($"{key}_missing", config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxRetries requires key [{key}_missing]");
    }
}
