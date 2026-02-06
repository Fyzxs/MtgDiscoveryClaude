using System;
using Lib.Universal.Primitives;

namespace Lib.Universal.Tests.Primitives;

[TestClass]
public sealed class UtcNowTimeInstantTests
{
    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnCurrentUtcTime()
    {
        // Arrange
        DateTime before = DateTime.UtcNow;
        UtcNowTimeInstant subject = new();

        // Act
        DateTime actual = subject.AsSystemType();

        // Assert
        DateTime after = DateTime.UtcNow;
        _ = actual.Should().BeOnOrAfter(before);
        _ = actual.Should().BeOnOrBefore(after);
    }

    [TestMethod, TestCategory("unit")]
    public void ImplicitConversion_ShouldReturnDateTime()
    {
        // Arrange
        DateTime before = DateTime.UtcNow;
        UtcNowTimeInstant subject = new();

        // Act
        DateTime actual = subject;

        // Assert
        DateTime after = DateTime.UtcNow;
        _ = actual.Should().BeOnOrAfter(before);
        _ = actual.Should().BeOnOrBefore(after);
    }
}
