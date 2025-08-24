using System;
using AwesomeAssertions;
using Lib.Shared.Abstractions.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Shared.Abstractions.Tests.Actions;

[TestClass]
public sealed class FilterActionResultTests
{
    [TestMethod, TestCategory("unit")]
    public void IsFilteredOut_WhenConstructedWithDefaultConstructor_ReturnsFalse()
    {
        // Arrange & Act
        FilterActionResult<string> subject = new();

        // Assert
        subject.IsFilteredOut().Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsNotFilteredOut_WhenConstructedWithDefaultConstructor_ReturnsTrue()
    {
        // Arrange & Act
        FilterActionResult<string> subject = new();

        // Assert
        ((IFilterActionResult<string>)subject).IsNotFilteredOut().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsFilteredOut_WhenConstructedWithFailureStatus_ReturnsTrue()
    {
        // Arrange
        string expectedStatus = "Failure message";

        // Act
        FilterActionResult<string> subject = new(expectedStatus);

        // Assert
        subject.IsFilteredOut().Should().BeTrue();
        ((IFilterActionResult<string>)subject).IsNotFilteredOut().Should().BeFalse();
        subject.FailureStatus().Should().Be(expectedStatus);
    }

    [TestMethod, TestCategory("unit")]
    public void FailureStatus_WhenConstructedWithNull_ReturnsNull()
    {
        // Arrange & Act
        FilterActionResult<string> subject = new(null!);

        // Assert
        subject.IsFilteredOut().Should().BeTrue();
        ((IFilterActionResult<string>)subject).IsNotFilteredOut().Should().BeFalse();
        subject.FailureStatus().Should().BeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void FailureStatus_WhenNotFilteredOut_ThrowsInvalidOperationException()
    {
        // Arrange
        FilterActionResult<string> subject = new();

        // Act & Assert
        Action act = () => subject.FailureStatus();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot retrieve failureStatus of non-filtered result.");
    }
}