using AwesomeAssertions;
using Lib.Scryfall.Ingestion.Filters;
using Lib.Scryfall.Ingestion.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Ingestion.Tests.Filters;

[TestClass]
public sealed class NonDigitalSetFilterTests
{
    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_NonDigitalSet_ReturnsTrue()
    {
        // Arrange
        NonDigitalSetFilter subject = new();
        ScryfallSetFake set = new(isDigital: false);

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_DigitalSet_ReturnsFalse()
    {
        // Arrange
        NonDigitalSetFilter subject = new();
        ScryfallSetFake set = new(isDigital: true);

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeFalse();
    }
}
