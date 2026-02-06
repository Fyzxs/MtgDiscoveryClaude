using AwesomeAssertions;
using Lib.Scryfall.Ingestion.Filters;
using Lib.Scryfall.Ingestion.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Ingestion.Tests.Filters;

[TestClass]
public sealed class SpecificSetsFilterTests
{
    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_NoSpecificSetsConfigured_ReturnsTrue()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                specificSets: new SpecificSetCodesFake()));
        SpecificSetsFilter subject = new(config);
        ScryfallSetFake set = new(code: "m21");

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_SetCodeInList_ReturnsTrue()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                specificSets: new SpecificSetCodesFake("m21", "neo")));
        SpecificSetsFilter subject = new(config);
        ScryfallSetFake set = new(code: "m21");

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_SetCodeNotInList_ReturnsFalse()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                specificSets: new SpecificSetCodesFake("m21", "neo")));
        SpecificSetsFilter subject = new(config);
        ScryfallSetFake set = new(code: "dmu");

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_UppercaseSetCode_MatchesLowercaseList()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                specificSets: new SpecificSetCodesFake("m21")));
        SpecificSetsFilter subject = new(config);
        ScryfallSetFake set = new(code: "M21");

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeTrue();
    }
}
