using Lib.Scryfall.Ingestion.Apis.Configurations;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Scryfall.Ingestion.Tests.Configurations;

[TestClass]
public class ConfigScryfallConfigurationTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithoutParameters_ShouldUseMonoStateConfig()
    {
        // Arrange & Act
        ConfigScryfallConfiguration subject = new();

        // Assert
        _ = subject.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithConfig_ShouldAcceptIConfig()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();

        // Act
        ConfigScryfallConfiguration subject = new(fakeConfig);

        // Assert
        _ = subject.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void ApiConfig_ShouldReturnIScryfallApiConfig()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        ConfigScryfallConfiguration subject = new(fakeConfig);

        // Act
        IScryfallApiConfig actual = subject.ApiConfig();

        // Assert
        _ = actual.Should().BeOfType<ConfigScryfallApiConfig>();
    }

    [TestMethod, TestCategory("unit")]
    public void RateLimitConfig_ShouldReturnIScryfallRateLimitConfig()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        ConfigScryfallConfiguration subject = new(fakeConfig);

        // Act
        IScryfallRateLimitConfig actual = subject.RateLimitConfig();

        // Assert
        _ = actual.Should().BeOfType<ConfigScryfallRateLimitConfig>();
    }

    [TestMethod, TestCategory("unit")]
    public void CacheConfig_ShouldReturnIScryfallCacheConfig()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        ConfigScryfallConfiguration subject = new(fakeConfig);

        // Act
        IScryfallCacheConfig actual = subject.CacheConfig();

        // Assert
        _ = actual.Should().BeOfType<ConfigScryfallCacheConfig>();
    }

    [TestMethod, TestCategory("unit")]
    public void RetryConfig_ShouldReturnIScryfallRetryConfig()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        ConfigScryfallConfiguration subject = new(fakeConfig);

        // Act
        IScryfallRetryConfig actual = subject.RetryConfig();

        // Assert
        _ = actual.Should().BeOfType<ConfigScryfallRetryConfig>();
    }

    [TestMethod, TestCategory("unit")]
    public void ApiConfig_ShouldReturnSameInstanceWhenCalledMultipleTimes()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        ConfigScryfallConfiguration subject = new(fakeConfig);

        // Act
        IScryfallApiConfig actual1 = subject.ApiConfig();
        IScryfallApiConfig actual2 = subject.ApiConfig();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
    }

    [TestMethod, TestCategory("unit")]
    public void RateLimitConfig_ShouldReturnSameInstanceWhenCalledMultipleTimes()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        ConfigScryfallConfiguration subject = new(fakeConfig);

        // Act
        IScryfallRateLimitConfig actual1 = subject.RateLimitConfig();
        IScryfallRateLimitConfig actual2 = subject.RateLimitConfig();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
    }

    [TestMethod, TestCategory("unit")]
    public void CacheConfig_ShouldReturnSameInstanceWhenCalledMultipleTimes()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        ConfigScryfallConfiguration subject = new(fakeConfig);

        // Act
        IScryfallCacheConfig actual1 = subject.CacheConfig();
        IScryfallCacheConfig actual2 = subject.CacheConfig();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
    }

    [TestMethod, TestCategory("unit")]
    public void RetryConfig_ShouldReturnSameInstanceWhenCalledMultipleTimes()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        ConfigScryfallConfiguration subject = new(fakeConfig);

        // Act
        IScryfallRetryConfig actual1 = subject.RetryConfig();
        IScryfallRetryConfig actual2 = subject.RetryConfig();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
    }
}
