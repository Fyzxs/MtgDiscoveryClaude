using System;
using System.Net;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.UserCards.Tests.Exceptions;

[TestClass]
public sealed class UserCardsAdapterExceptionTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_Default_CreatesInstanceWithDefaultMessage()
    {
        // Act
        UserCardsAdapterException actual = new();

        // Assert
        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        actual.StatusMessage.Should().Be("UserCards adapter operation failed");
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessage_CreatesInstanceWithSpecifiedMessage()
    {
        // Arrange
        string message = "Custom error message";

        // Act
        UserCardsAdapterException actual = new(message);

        // Assert
        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        actual.StatusMessage.Should().Be(message);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessageAndInnerException_CreatesInstanceWithBoth()
    {
        // Arrange
        string message = "Custom error with inner exception";
        Exception innerException = new InvalidOperationException("Inner exception");

        // Act
        UserCardsAdapterException actual = new(message, innerException);

        // Assert
        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        actual.StatusMessage.Should().Be(message);
        actual.InnerException.Should().Be(innerException);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_InheritsFromOperationException()
    {
        // Act
        UserCardsAdapterException actual = new();

        // Assert
        actual.Should().BeAssignableTo<OperationException>();
    }
}
