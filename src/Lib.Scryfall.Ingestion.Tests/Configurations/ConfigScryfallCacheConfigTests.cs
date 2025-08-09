using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Scryfall.Ingestion.Tests.Configurations;

[TestClass]
public class ConfigScryfallCacheConfigTests
{
    [TestMethod, TestCategory("unit")]
    public void CardTtlHours_ShouldReturnConfigScryfallCacheTtl()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Cache:CardTtlHours"] = "24";
        ConfigScryfallCacheConfig subject = new(fakeConfig);

        // Act
        ScryfallCacheTtl actual = subject.CardTtlHours();

        // Assert
        _ = actual.AsSystemType().Should().Be(24);
    }

    [TestMethod, TestCategory("unit")]
    public void SetTtlHours_ShouldReturnConfigScryfallCacheTtl()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Cache:SetTtlHours"] = "168";
        ConfigScryfallCacheConfig subject = new(fakeConfig);

        // Act
        ScryfallCacheTtl actual = subject.SetTtlHours();

        // Assert
        _ = actual.AsSystemType().Should().Be(168);
    }

    [TestMethod, TestCategory("unit")]
    public void MaxCacheSize_ShouldReturnConfigScryfallMaxCacheSize()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Cache:MaxCacheSize"] = "500";
        ConfigScryfallCacheConfig subject = new(fakeConfig);

        // Act
        ScryfallMaxCacheSize actual = subject.MaxCacheSize();

        // Assert
        _ = actual.AsSystemType().Should().Be(500);
    }

    [TestMethod, TestCategory("unit")]
    public void CardTtlHours_ShouldCreateNewInstanceEachTime()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Cache:CardTtlHours"] = "24";
        ConfigScryfallCacheConfig subject = new(fakeConfig);

        // Act
        ScryfallCacheTtl actual1 = subject.CardTtlHours();
        ScryfallCacheTtl actual2 = subject.CardTtlHours();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
        _ = actual1.AsSystemType().Should().Be(actual2.AsSystemType());
    }

    [TestMethod, TestCategory("unit")]
    public void SetTtlHours_ShouldCreateNewInstanceEachTime()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Cache:SetTtlHours"] = "168";
        ConfigScryfallCacheConfig subject = new(fakeConfig);

        // Act
        ScryfallCacheTtl actual1 = subject.SetTtlHours();
        ScryfallCacheTtl actual2 = subject.SetTtlHours();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
        _ = actual1.AsSystemType().Should().Be(actual2.AsSystemType());
    }

    [TestMethod, TestCategory("unit")]
    public void MaxCacheSize_ShouldCreateNewInstanceEachTime()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Cache:MaxCacheSize"] = "500";
        ConfigScryfallCacheConfig subject = new(fakeConfig);

        // Act
        ScryfallMaxCacheSize actual1 = subject.MaxCacheSize();
        ScryfallMaxCacheSize actual2 = subject.MaxCacheSize();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
        _ = actual1.AsSystemType().Should().Be(actual2.AsSystemType());
    }
}
