using System;
using System.Dynamic;
using AwesomeAssertions;
using Lib.Scryfall.Ingestion.Filters;
using Lib.Scryfall.Ingestion.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Ingestion.Tests.Filters;

[TestClass]
public sealed class ReleasedAfterDateFilterTests
{
    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_NoFilterDateConfigured_ReturnsTrue()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                releasedAfter: new ReleasedAfterDateFake()));
        ReleasedAfterDateFilter subject = new(config);
        ScryfallSetFake set = new();

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_SetReleasedAfterFilterDate_ReturnsTrue()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                releasedAfter: new ReleasedAfterDateFake(new DateTime(2020, 1, 1))));
        ReleasedAfterDateFilter subject = new(config);
        dynamic data = new ExpandoObject();
        data.released_at = "2021-06-15";
        ScryfallSetFake set = new(data: (object)data);

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_SetReleasedOnFilterDate_ReturnsTrue()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                releasedAfter: new ReleasedAfterDateFake(new DateTime(2020, 6, 15))));
        ReleasedAfterDateFilter subject = new(config);
        dynamic data = new ExpandoObject();
        data.released_at = "2020-06-15";
        ScryfallSetFake set = new(data: (object)data);

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_SetReleasedBeforeFilterDate_ReturnsFalse()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                releasedAfter: new ReleasedAfterDateFake(new DateTime(2022, 1, 1))));
        ReleasedAfterDateFilter subject = new(config);
        dynamic data = new ExpandoObject();
        data.released_at = "2020-06-15";
        ScryfallSetFake set = new(data: (object)data);

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_SetWithNoReleaseDate_ReturnsFalse()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                releasedAfter: new ReleasedAfterDateFake(new DateTime(2020, 1, 1))));
        ReleasedAfterDateFilter subject = new(config);
        dynamic data = new ExpandoObject();
        data.released_at = (string)null;
        ScryfallSetFake set = new(data: (object)data);

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldInclude_SetWithInvalidReleaseDate_ReturnsFalse()
    {
        // Arrange
        ScryfallIngestionConfigurationFake config = new(
            new ScryfallProcessingConfigFake(
                releasedAfter: new ReleasedAfterDateFake(new DateTime(2020, 1, 1))));
        ReleasedAfterDateFilter subject = new(config);
        dynamic data = new ExpandoObject();
        data.released_at = "not-a-date";
        ScryfallSetFake set = new(data: (object)data);

        // Act
        bool actual = subject.ShouldInclude(set);

        // Assert
        actual.Should().BeFalse();
    }
}
