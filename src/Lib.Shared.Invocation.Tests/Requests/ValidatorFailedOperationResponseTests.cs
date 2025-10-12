using System.Net;
using Lib.Shared.Invocation.Requests;

namespace Lib.Shared.Invocation.Tests.Requests;

[TestClass]
public sealed class ValidatorFailedOperationResponseTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessage_SetsBadRequestStatusCode()
    {
        // Arrange
        string testMessage = "Validation failed";

        // Act
        ValidatorFailedOperationResponse<string> subject = new(testMessage);

        // Assert
        subject.Status.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessage_SetsIsFailureToTrue()
    {
        // Arrange
        string testMessage = "Validation error";

        // Act
        ValidatorFailedOperationResponse<int> subject = new(testMessage);

        // Assert
        subject.IsFailure.Should().BeTrue();
        subject.IsSuccess.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessage_CreatesValidatorOperationException()
    {
        // Arrange
        string expectedMessage = "Custom validation message";

        // Act
        ValidatorFailedOperationResponse<bool> subject = new(expectedMessage);

        // Assert
        subject.OuterException.Should().NotBeNull();
        subject.OuterException.Should().BeOfType<ValidatorOperationException>();
        subject.OuterException.StatusMessage.Should().Be(expectedMessage);
        subject.OuterException.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithEmptyMessage_SetsEmptyStatusMessage()
    {
        // Arrange & Act
        ValidatorFailedOperationResponse<object> subject = new(string.Empty);

        // Assert
        subject.Status.Should().Be(HttpStatusCode.BadRequest);
        subject.OuterException.StatusMessage.Should().Be(string.Empty);
        subject.IsFailure.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessage_ResponseDataIsDefault()
    {
        // Arrange
        const string TestMessage = "Test message";

        // Act
        ValidatorFailedOperationResponse<string> subject = new(TestMessage);

        // Assert
        subject.ResponseData.Should().BeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithGenericType_PreservesGenericType()
    {
        // Arrange
        const string TestMessage = "Generic type test";

        // Act
        ValidatorFailedOperationResponse<List<int>> subject = new(TestMessage);

        // Assert
        subject.Status.Should().Be(HttpStatusCode.BadRequest);
        subject.ResponseData.Should().BeNull();
        subject.IsFailure.Should().BeTrue();
    }
}
