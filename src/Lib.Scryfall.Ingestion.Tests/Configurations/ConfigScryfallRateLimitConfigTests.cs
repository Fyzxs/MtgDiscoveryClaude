using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Scryfall.Ingestion.Tests.Configurations;

[TestClass]
public class ConfigScryfallRateLimitConfigTests
{
    [TestMethod, TestCategory("unit")]
    public void RequestsPerSecond_ShouldReturnConfigScryfallRequestsPerSecond()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:RateLimit:RequestsPerSecond"] = "10";
        ConfigScryfallRateLimitConfig subject = new(fakeConfig);

        // Act
        ScryfallRequestsPerSecond actual = subject.RequestsPerSecond();

        // Assert
        _ = actual.AsSystemType().Should().Be(10);
    }

    [TestMethod, TestCategory("unit")]
    public void BurstSize_ShouldReturnConfigScryfallBurstSize()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:RateLimit:BurstSize"] = "15";
        ConfigScryfallRateLimitConfig subject = new(fakeConfig);

        // Act
        ScryfallBurstSize actual = subject.BurstSize();

        // Assert
        _ = actual.AsSystemType().Should().Be(15);
    }

    [TestMethod, TestCategory("unit")]
    public void ThrottleWarningThreshold_ShouldReturnConfigScryfallThrottleWarningThreshold()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:RateLimit:ThrottleWarningThreshold"] = "0.8";
        ConfigScryfallRateLimitConfig subject = new(fakeConfig);

        // Act
        ScryfallThrottleWarningThreshold actual = subject.ThrottleWarningThreshold();

        // Assert
        _ = actual.AsSystemType().Should().Be(0.8);
    }

    [TestMethod, TestCategory("unit")]
    public void RequestsPerSecond_ShouldCreateNewInstanceEachTime()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:RateLimit:RequestsPerSecond"] = "10";
        ConfigScryfallRateLimitConfig subject = new(fakeConfig);

        // Act
        ScryfallRequestsPerSecond actual1 = subject.RequestsPerSecond();
        ScryfallRequestsPerSecond actual2 = subject.RequestsPerSecond();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
        _ = actual1.AsSystemType().Should().Be(actual2.AsSystemType());
    }

    [TestMethod, TestCategory("unit")]
    public void BurstSize_ShouldCreateNewInstanceEachTime()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:RateLimit:BurstSize"] = "15";
        ConfigScryfallRateLimitConfig subject = new(fakeConfig);

        // Act
        ScryfallBurstSize actual1 = subject.BurstSize();
        ScryfallBurstSize actual2 = subject.BurstSize();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
        _ = actual1.AsSystemType().Should().Be(actual2.AsSystemType());
    }

    [TestMethod, TestCategory("unit")]
    public void ThrottleWarningThreshold_ShouldCreateNewInstanceEachTime()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:RateLimit:ThrottleWarningThreshold"] = "0.8";
        ConfigScryfallRateLimitConfig subject = new(fakeConfig);

        // Act
        ScryfallThrottleWarningThreshold actual1 = subject.ThrottleWarningThreshold();
        ScryfallThrottleWarningThreshold actual2 = subject.ThrottleWarningThreshold();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
        _ = actual1.AsSystemType().Should().Be(actual2.AsSystemType());
    }
}
