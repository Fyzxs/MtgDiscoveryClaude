using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Shared.Invocation.Tests.Exceptions;

[TestClass]
public sealed class OperationExceptionTests
{
#pragma warning disable CA1032 // Implement standard exception constructors
    private sealed class TestOperationException : OperationException
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        public TestOperationException(HttpStatusCode statusCode, string statusMessage, Exception? innerException = null)
            : base(statusCode, statusMessage, innerException)
        {
        }
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithStatusCodeAndMessage_SetsProperties()
    {
        // Arrange
        HttpStatusCode expectedStatusCode = HttpStatusCode.InternalServerError;
        string expectedMessage = "Test error";

        // Act
        TestOperationException subject = new(expectedStatusCode, expectedMessage);

        // Assert
        subject.StatusCode.Should().Be(expectedStatusCode);
        subject.StatusMessage.Should().Be(expectedMessage);
        subject.Message.Should().Be("[status=InternalServerError] [message=Test error]");
        subject.InnerException.Should().BeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithInnerException_SetsInnerException()
    {
        // Arrange
        HttpStatusCode statusCode = HttpStatusCode.BadRequest;
        string message = "Test error";
        Exception innerException = new InvalidOperationException("Inner error");

        // Act
        TestOperationException subject = new(statusCode, message, innerException);

        // Assert
        subject.StatusCode.Should().Be(statusCode);
        subject.StatusMessage.Should().Be(message);
        subject.InnerException.Should().BeSameAs(innerException);
    }
}

[TestClass]
public sealed class BadRequestOperationExceptionTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessage_SetsBadRequestStatusCode()
    {
        // Arrange
        string expectedMessage = "Bad request error";

        // Act
        BadRequestOperationException subject = new(expectedMessage);

        // Assert
        subject.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        subject.StatusMessage.Should().Be(expectedMessage);
        subject.Message.Should().Be("[status=BadRequest] [message=Bad request error]");
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithEmptyMessage_SetsEmptyStatusMessage()
    {
        // Arrange & Act
        BadRequestOperationException subject = new(string.Empty);

        // Assert
        subject.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        subject.StatusMessage.Should().Be(string.Empty);
    }
}
