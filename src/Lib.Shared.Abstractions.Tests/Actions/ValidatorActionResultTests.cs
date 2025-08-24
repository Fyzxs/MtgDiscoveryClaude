using System;
using AwesomeAssertions;
using Lib.Shared.Abstractions.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Shared.Abstractions.Tests.Actions;

[TestClass]
public sealed class ValidatorActionResultTests
{
    [TestMethod, TestCategory("unit")]
    public void IsValid_WhenConstructedWithValidResult_ReturnsTrue()
    {
        // Arrange & Act
        ValidatorActionResult<string> subject = new();

        // Assert
        subject.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsNotValid_WhenConstructedWithValidResult_ReturnsFalse()
    {
        // Arrange & Act
        ValidatorActionResult<string> subject = new();

        // Assert
        ((IValidatorActionResult<string>)subject).IsNotValid().Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void FailureStatus_WhenConstructedWithFailureStatus_ReturnsProvidedStatus()
    {
        // Arrange
        string expectedStatus = "Failure message";

        // Act
        ValidatorActionResult<string> subject = new(expectedStatus);

        // Assert
        subject.IsValid().Should().BeFalse();
        ((IValidatorActionResult<string>)subject).IsNotValid().Should().BeTrue();
        subject.FailureStatus().Should().Be(expectedStatus);
    }

    [TestMethod, TestCategory("unit")]
    public void FailureStatus_WhenConstructedWithNull_ReturnsNull()
    {
        // Arrange & Act
        ValidatorActionResult<string> subject = new(null!);

        // Assert
        subject.IsValid().Should().BeFalse();
        ((IValidatorActionResult<string>)subject).IsNotValid().Should().BeTrue();
        subject.FailureStatus().Should().BeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void FailureStatus_WhenValid_ThrowsInvalidOperationException()
    {
        // Arrange
        ValidatorActionResult<string> subject = new();

        // Act & Assert
        Action act = () => subject.FailureStatus();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot retrieve failureStatus of Valid result.");
    }
}