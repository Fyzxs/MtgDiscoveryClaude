using Lib.Scryfall.Ingestion.Apis.Configurations.Values;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Scryfall.Ingestion.Tests.Configurations;

[TestClass]
public class ConfigScryfallApiConfigTests
{
    [TestMethod, TestCategory("unit")]
    public void BaseUrl_ShouldReturnConfigScryfallApiBaseUrl()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Api:BaseUrl"] = "https://api.scryfall.com";
        ConfigScryfallApiConfig subject = new(fakeConfig);

        // Act
        ScryfallApiBaseUrl actual = subject.BaseUrl();

        // Assert
        _ = actual.AsSystemType().Should().Be("https://api.scryfall.com");
    }

    [TestMethod, TestCategory("unit")]
    public void TimeoutSeconds_ShouldReturnConfigScryfallApiTimeout()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Api:TimeoutSeconds"] = "30";
        ConfigScryfallApiConfig subject = new(fakeConfig);

        // Act
        ScryfallApiTimeout actual = subject.TimeoutSeconds();

        // Assert
        _ = actual.AsSystemType().Should().Be(30);
    }

    [TestMethod, TestCategory("unit")]
    public void BaseUrl_ShouldCreateNewInstanceEachTime()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Api:BaseUrl"] = "https://api.scryfall.com";
        ConfigScryfallApiConfig subject = new(fakeConfig);

        // Act
        ScryfallApiBaseUrl actual1 = subject.BaseUrl();
        ScryfallApiBaseUrl actual2 = subject.BaseUrl();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
        _ = actual1.AsSystemType().Should().Be(actual2.AsSystemType());
    }

    [TestMethod, TestCategory("unit")]
    public void TimeoutSeconds_ShouldCreateNewInstanceEachTime()
    {
        // Arrange
        IConfig fakeConfig = new ConfigFake();
        fakeConfig["ScryfallConfig:Api:TimeoutSeconds"] = "30";
        ConfigScryfallApiConfig subject = new(fakeConfig);

        // Act
        ScryfallApiTimeout actual1 = subject.TimeoutSeconds();
        ScryfallApiTimeout actual2 = subject.TimeoutSeconds();

        // Assert
        _ = actual1.Should().NotBeSameAs(actual2);
        _ = actual1.AsSystemType().Should().Be(actual2.AsSystemType());
    }
}
