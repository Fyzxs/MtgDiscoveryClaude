using System;
using System.Dynamic;
using AwesomeAssertions;
using Lib.Scryfall.Ingestion.Filters;
using Lib.Scryfall.Ingestion.Tests.Fakes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Ingestion.Tests.Filters;

[TestClass]
public sealed class PreviewSetFilterTests
{
    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_PastReleaseDate_ReturnsTrue()
    {
        // Arrange
        ILogger logger = NullLogger.Instance;
        PreviewSetFilter subject = new(logger);
        dynamic data = new ExpandoObject();
        data.released_at = "2020-01-01";
        ScryfallSetFake set = new(data: (object)data);

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_FutureReleaseDate_ReturnsFalse()
    {
        // Arrange
        ILogger logger = NullLogger.Instance;
        PreviewSetFilter subject = new(logger);
        string futureDate = DateTime.UtcNow.AddYears(1).ToString("yyyy-MM-dd");
        dynamic data = new ExpandoObject();
        data.released_at = futureDate;
        ScryfallSetFake set = new(data: (object)data);

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_NoReleaseDate_ReturnsTrue()
    {
        // Arrange
        ILogger logger = NullLogger.Instance;
        PreviewSetFilter subject = new(logger);
        dynamic data = new ExpandoObject();
        data.released_at = (string)null;
        ScryfallSetFake set = new(data: (object)data);

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_EmptyReleaseDate_ReturnsTrue()
    {
        // Arrange
        ILogger logger = NullLogger.Instance;
        PreviewSetFilter subject = new(logger);
        dynamic data = new ExpandoObject();
        data.released_at = "";
        ScryfallSetFake set = new(data: (object)data);

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_InvalidReleaseDate_ReturnsTrue()
    {
        // Arrange
        ILogger logger = NullLogger.Instance;
        PreviewSetFilter subject = new(logger);
        dynamic data = new ExpandoObject();
        data.released_at = "not-a-date";
        ScryfallSetFake set = new(data: (object)data);

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeTrue();
    }
}
