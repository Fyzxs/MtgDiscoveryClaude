using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Scryfall.Ingestion.Tests.Configurations;

[TestClass]
public class ConfigScryfallRetryConfigTests
{
    [TestMethod, TestCategory("unit")]
    public void MaxRetries_ShouldReturnConfigScryfallMaxRetries()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Retry:MaxRetries"] = "3";
        ConfigScryfallRetryConfig subject = new(fakeConfig);

        // Act
        ScryfallMaxRetries actual = subject.MaxRetries();

        // Assert
        _ = actual.AsSystemType().Should().Be(3);
    }

    [TestMethod, TestCategory("unit")]
    public void InitialDelayMs_ShouldReturnConfigScryfallRetryDelay()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Retry:InitialDelayMs"] = "1000";
        ConfigScryfallRetryConfig subject = new(fakeConfig);

        // Act
        ScryfallRetryDelay actual = subject.InitialDelayMs();

        // Assert
        _ = actual.AsSystemType().Should().Be(1000);
    }

    [TestMethod, TestCategory("unit")]
    public void MaxDelayMs_ShouldReturnConfigScryfallRetryDelay()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Retry:MaxDelayMs"] = "32000";
        ConfigScryfallRetryConfig subject = new(fakeConfig);

        // Act
        ScryfallRetryDelay actual = subject.MaxDelayMs();

        // Assert
        _ = actual.AsSystemType().Should().Be(32000);
    }

    [TestMethod, TestCategory("unit")]
    public void BackoffMultiplier_ShouldReturnConfigScryfallBackoffMultiplier()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Retry:BackoffMultiplier"] = "2.0";
        ConfigScryfallRetryConfig subject = new(fakeConfig);

        // Act
        ScryfallBackoffMultiplier actual = subject.BackoffMultiplier();

        // Assert
        _ = actual.AsSystemType().Should().Be(2.0);
    }

    [TestMethod, TestCategory("unit")]
    public void MaxRetries_ShouldCreateNewInstanceEachTime()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Retry:MaxRetries"] = "3";
        ConfigScryfallRetryConfig subject = new(fakeConfig);

        // Act
        ScryfallMaxRetries actual1 = subject.MaxRetries();
        ScryfallMaxRetries actual2 = subject.MaxRetries();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
        _ = actual1.AsSystemType().Should().Be(actual2.AsSystemType());
    }

    [TestMethod, TestCategory("unit")]
    public void InitialDelayMs_ShouldCreateNewInstanceEachTime()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Retry:InitialDelayMs"] = "1000";
        ConfigScryfallRetryConfig subject = new(fakeConfig);

        // Act
        ScryfallRetryDelay actual1 = subject.InitialDelayMs();
        ScryfallRetryDelay actual2 = subject.InitialDelayMs();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
        _ = actual1.AsSystemType().Should().Be(actual2.AsSystemType());
    }

    [TestMethod, TestCategory("unit")]
    public void MaxDelayMs_ShouldCreateNewInstanceEachTime()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Retry:MaxDelayMs"] = "32000";
        ConfigScryfallRetryConfig subject = new(fakeConfig);

        // Act
        ScryfallRetryDelay actual1 = subject.MaxDelayMs();
        ScryfallRetryDelay actual2 = subject.MaxDelayMs();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
        _ = actual1.AsSystemType().Should().Be(actual2.AsSystemType());
    }

    [TestMethod, TestCategory("unit")]
    public void BackoffMultiplier_ShouldCreateNewInstanceEachTime()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Retry:BackoffMultiplier"] = "2.0";
        ConfigScryfallRetryConfig subject = new(fakeConfig);

        // Act
        ScryfallBackoffMultiplier actual1 = subject.BackoffMultiplier();
        ScryfallBackoffMultiplier actual2 = subject.BackoffMultiplier();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
        _ = actual1.AsSystemType().Should().Be(actual2.AsSystemType());
    }
}
