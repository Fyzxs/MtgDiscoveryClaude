using System;
using Lib.Universal.Primitives;

namespace Lib.Universal.Tests.Primitives;

[TestClass]
public sealed class Iso8601UtcNowStringTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnIso8601FormattedString()
    {
        // Arrange
        Iso8601UtcNowString subject = new();

        // Act
        string actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Contain("T");
        _ = actual.Should().EndWith("Z");
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldBeParseableAsDateTime()
    {
        // Arrange
        DateTime before = DateTime.UtcNow;
        Iso8601UtcNowString subject = new();

        // Act
        string actual = subject.AsSystemType();
        DateTime parsed = DateTime.Parse(actual, null, System.Globalization.DateTimeStyles.RoundtripKind);

        // Assert
        DateTime after = DateTime.UtcNow;
        _ = parsed.Should().BeOnOrAfter(before.AddSeconds(-1));
        _ = parsed.Should().BeOnOrBefore(after.AddSeconds(1));
    }

    [TestMethod, TestCategory("unit")]
    public void ImplicitConversion_ShouldReturnIso8601FormattedString()
    {
        // Arrange
        Iso8601UtcNowString subject = new();

        // Act
        string actual = subject;

        // Assert
        _ = actual.Should().Contain("T");
        _ = actual.Should().EndWith("Z");
    }
}
