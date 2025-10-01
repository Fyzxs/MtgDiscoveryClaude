using System.Net;
using Lib.Shared.Invocation.Requests;

namespace Lib.Shared.Invocation.Tests.Requests;

[TestClass]
public sealed class ValidatorOperationExceptionTests
{
    private sealed class ValidatorOperationExceptionTypeWrapper
    {
        private readonly ValidatorOperationException _exception;

        public ValidatorOperationExceptionTypeWrapper(string message) => _exception = new ValidatorOperationException(message);

        public HttpStatusCode StatusCode => _exception.StatusCode;
        public string StatusMessage => _exception.StatusMessage;
        public string Message => _exception.Message;
        public Exception? InnerException => _exception.InnerException;
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessage_SetsBadRequestStatusCode()
    {
        // Arrange
        const string TestMessage = "Validation failed";

        // Act
        ValidatorOperationExceptionTypeWrapper subject = new(TestMessage);

        // Assert
        subject.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessage_SetsStatusMessage()
    {
        // Arrange
        const string ExpectedMessage = "Custom validation message";

        // Act
        ValidatorOperationExceptionTypeWrapper subject = new(ExpectedMessage);

        // Assert
        subject.StatusMessage.Should().Be(ExpectedMessage);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessage_SetsFormattedMessage()
    {
        // Arrange
        const string TestMessage = "Validation error occurred";

        // Act
        ValidatorOperationExceptionTypeWrapper subject = new(TestMessage);

        // Assert
        subject.Message.Should().Be("[status=BadRequest] [message=Validation error occurred]");
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithEmptyMessage_SetsEmptyStatusMessage()
    {
        // Arrange & Act
        ValidatorOperationExceptionTypeWrapper subject = new(string.Empty);

        // Assert
        subject.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        subject.StatusMessage.Should().Be(string.Empty);
        subject.Message.Should().Be("[status=BadRequest] [message=]");
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessage_InnerExceptionIsNull()
    {
        // Arrange
        const string TestMessage = "Test validation message";

        // Act
        ValidatorOperationExceptionTypeWrapper subject = new(TestMessage);

        // Assert
        subject.InnerException.Should().BeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_AlwaysSetsBadRequest_RegardlessOfMessage()
    {
        // Arrange & Act
        ValidatorOperationExceptionTypeWrapper subject1 = new("Message 1");
        ValidatorOperationExceptionTypeWrapper subject2 = new("Completely different message");
        ValidatorOperationExceptionTypeWrapper subject3 = new("Yet another message type");

        // Assert
        subject1.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        subject2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        subject3.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithWhitespaceMessage_PreservesWhitespace()
    {
        // Arrange
        const string WhitespaceMessage = "   ";

        // Act
        ValidatorOperationExceptionTypeWrapper subject = new(WhitespaceMessage);

        // Assert
        subject.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        subject.StatusMessage.Should().Be(WhitespaceMessage);
        subject.Message.Should().Be("[status=BadRequest] [message=   ]");
    }
}
