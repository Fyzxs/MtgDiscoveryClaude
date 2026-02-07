using AwesomeAssertions;
using Lib.Scryfall.Ingestion.Filters;
using Lib.Scryfall.Ingestion.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Ingestion.Tests.Filters;

[TestClass]
public sealed class MaxSetsFilterTests
{
    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_UnlimitedConfig_ReturnsTrue()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                maxSets: new MaxSetsToProcessFake(0, isUnlimited: true)));
        MaxSetsFilter subject = new(config);
        ScryfallSetFake set = new();

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_UnlimitedConfig_AlwaysReturnsTrue()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                maxSets: new MaxSetsToProcessFake(0, isUnlimited: true)));
        MaxSetsFilter subject = new(config);
        ScryfallSetFake set = new();

        // Act & Assert
        subject.ShouldInclude(set).Should().BeTrue();
        subject.ShouldInclude(set).Should().BeTrue();
        subject.ShouldInclude(set).Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_MaxOfTwo_IncludesFirstTwo()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                maxSets: new MaxSetsToProcessFake(2)));
        MaxSetsFilter subject = new(config);
        ScryfallSetFake set = new();

        // Act & Assert
        subject.ShouldInclude(set).Should().BeTrue();
        subject.ShouldInclude(set).Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_MaxOfTwo_ExcludesThird()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                maxSets: new MaxSetsToProcessFake(2)));
        MaxSetsFilter subject = new(config);
        ScryfallSetFake set = new();

        // Act
        subject.ShouldInclude(set);
        subject.ShouldInclude(set);
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_MaxOfOne_ExcludesSecond()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                maxSets: new MaxSetsToProcessFake(1)));
        MaxSetsFilter subject = new(config);
        ScryfallSetFake set = new();

        // Act
        subject.ShouldInclude(set);
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeFalse();
    }
}
