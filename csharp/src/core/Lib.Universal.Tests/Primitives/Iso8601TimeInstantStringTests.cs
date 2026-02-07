using System;
using Lib.Universal.Primitives;

namespace Lib.Universal.Tests.Primitives;

[TestClass]
public sealed class Iso8601TimeInstantStringTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnIso8601FormattedString()
    {
        // Arrange
        DateTime frozen = new(2026, 1, 15, 12, 30, 45, DateTimeKind.Utc);
        FrozenTimeInstant timeInstant = new(frozen);
        Iso8601TimeInstantString subject = new(timeInstant);

        // Act
        string actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be("2026-01-15T12:30:45.0000000Z");
    }

    [TestMethod, TestCategory("unit")]
    public void ImplicitConversion_ShouldReturnIso8601FormattedString()
    {
        // Arrange
        DateTime frozen = new(2026, 1, 15, 12, 30, 45, DateTimeKind.Utc);
        FrozenTimeInstant timeInstant = new(frozen);
        Iso8601TimeInstantString subject = new(timeInstant);

        // Act
        string actual = subject;

        // Assert
        _ = actual.Should().Be("2026-01-15T12:30:45.0000000Z");
    }
}
