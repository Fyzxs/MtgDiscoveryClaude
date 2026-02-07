using AwesomeAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Universal.Tests;

[TestClass]
public sealed class SmokeTests
{
    [TestMethod, TestCategory("unit")]
    public void Smoke_ReturnsTrue()
    {
        // Arrange
        bool expected = true;

        // Act
        bool actual = expected;

        // Assert
        actual.Should().BeTrue();
    }
}
