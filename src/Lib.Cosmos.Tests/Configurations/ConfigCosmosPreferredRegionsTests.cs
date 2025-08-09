using System.Collections.Generic;
using Lib.Cosmos.Configurations;
using Lib.Cosmos.Tests.Fakes;
using Lib.Universal.Configurations;
using TestConvenience.Core.Fakes;

namespace Lib.Cosmos.Tests.Configurations;

[TestClass]
public class ConfigCosmosPreferredRegionsTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnSingleRegion()
    {
        // Arrange
        string sourceKey = "test:preferred:regions";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "East US";
        ConfigCosmosPreferredRegions subject = new(sourceKey, fakeConfig);

        // Act
        IReadOnlyList<string> actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().HaveCount(1);
        _ = actual[0].Should().Be("East US");
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnMultipleRegions_WhenCommaSeparated()
    {
        // Arrange
        string sourceKey = "test:preferred:regions";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "East US,West US,Central US";
        ConfigCosmosPreferredRegions subject = new(sourceKey, fakeConfig);

        // Act
        IReadOnlyList<string> actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().HaveCount(3);
        _ = actual[0].Should().Be("East US");
        _ = actual[1].Should().Be("West US");
        _ = actual[2].Should().Be("Central US");
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldTrimWhitespace()
    {
        // Arrange
        string sourceKey = "test:preferred:regions";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = " East US , West US , Central US ";
        ConfigCosmosPreferredRegions subject = new(sourceKey, fakeConfig);

        // Act
        IReadOnlyList<string> actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().HaveCount(3);
        _ = actual[0].Should().Be("East US");
        _ = actual[1].Should().Be("West US");
        _ = actual[2].Should().Be("Central US");
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnEmptyList_WhenValueIsEmpty()
    {
        // Arrange
        string sourceKey = "test:preferred:regions";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "";
        ConfigCosmosPreferredRegions subject = new(sourceKey, fakeConfig);

        // Act
        IReadOnlyList<string> actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldRemoveEmptyEntries()
    {
        // Arrange
        string sourceKey = "test:preferred:regions";
        IConfig fakeConfig = new ConfigFake();
        fakeConfig[sourceKey] = "East US,,West US";
        ConfigCosmosPreferredRegions subject = new(sourceKey, fakeConfig);

        // Act
        IReadOnlyList<string> actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().HaveCount(2);
        _ = actual[0].Should().Be("East US");
        _ = actual[1].Should().Be("West US");
    }
}
