using System;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Ingestion.Configurations.Values;
using TestConvenience.Core.Fakes;

namespace Lib.Scryfall.Ingestion.Tests.Configurations.Values;

[TestClass]
public sealed class ConfigScryfallMaxCacheSizeTests
{
    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_ValidMaxCacheSize_ReturnsMaxCacheSize()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "512" };
        ConfigScryfallMaxCacheSize configValue = new(key, config);

        // Act
        int actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(512);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MinimumValue_ReturnsValue()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "1" };
        ConfigScryfallMaxCacheSize configValue = new(key, config);

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
        ConfigFake config = new() { [key] = "10000" };
        ConfigScryfallMaxCacheSize configValue = new(key, config);

        // Act
        int actual = configValue.AsSystemType();

        // Assert
        _ = actual.Should().Be(10000);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_NullValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = null! };
        ConfigScryfallMaxCacheSize configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxCacheSize requires key [{key}]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_EmptyValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "" };
        ConfigScryfallMaxCacheSize configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxCacheSize requires key [{key}]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_WhitespaceValue_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "   " };
        ConfigScryfallMaxCacheSize configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxCacheSize requires key [{key}]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_InvalidFormat_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "not-a-number" };
        ConfigScryfallMaxCacheSize configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxCacheSize Invalid value [{key}=not-a-number]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_BelowMinimum_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "0" };
        ConfigScryfallMaxCacheSize configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxCacheSize value must be between 1 and 10000 MB [{key}=0]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_AboveMaximum_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new() { [key] = "10001" };
        ConfigScryfallMaxCacheSize configValue = new(key, config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxCacheSize value must be between 1 and 10000 MB [{key}=10001]");
    }

    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_MissingKey_ThrowsScryfallConfigurationException()
    {
        // Arrange
        string key = Guid.NewGuid().ToString();
        ConfigFake config = new();
        ConfigScryfallMaxCacheSize configValue = new($"{key}_missing", config);

        // Act
        Action act = () => configValue.AsSystemType();

        // Assert
        act.Should().Throw<ScryfallConfigurationException>()
            .WithMessage($"ConfigScryfallMaxCacheSize requires key [{key}_missing]");
    }
}
