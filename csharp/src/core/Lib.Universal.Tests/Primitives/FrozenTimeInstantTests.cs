using System;
using Lib.Universal.Primitives;

namespace Lib.Universal.Tests.Primitives;

[TestClass]
public sealed class FrozenTimeInstantTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnFrozenTime()
    {
        // Arrange
        DateTime expected = new(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        FrozenTimeInstant subject = new(expected);

        // Act
        DateTime actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(expected);
    }

    [TestMethod, TestCategory("unit")]
    public void ImplicitConversion_ShouldReturnFrozenTime()
    {
        // Arrange
        DateTime expected = new(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        FrozenTimeInstant subject = new(expected);

        // Act
        DateTime actual = subject;

        // Assert
        _ = actual.Should().Be(expected);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_CalledMultipleTimes_ShouldReturnSameValue()
    {
        // Arrange
        DateTime expected = new(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        FrozenTimeInstant subject = new(expected);

        // Act
        DateTime first = subject.AsSystemType();
        DateTime second = subject.AsSystemType();

        // Assert
        _ = first.Should().Be(expected);
        _ = second.Should().Be(expected);
    }
}
