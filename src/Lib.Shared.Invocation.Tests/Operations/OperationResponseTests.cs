using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;

namespace Lib.Shared.Invocation.Tests.Operations;

[TestClass]
public sealed class OperationResponseMessageTests
{
    private sealed class TestOperationResponseMessage : OperationResponseMessage
    {
        private readonly string _value;

        public TestOperationResponseMessage(string value) => _value = value;

        public override string AsSystemType() => _value;
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ReturnsExpectedValue()
    {
        // Arrange
        string expectedValue = "Test message";
        TestOperationResponseMessage subject = new(expectedValue);

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be(expectedValue);
    }
}

[TestClass]
public sealed class SuccessOperationResponseTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithResponseData_SetsPropertiesCorrectly()
    {
        // Arrange
        string expectedData = "Success data";

        // Act
        SuccessOperationResponse<string> subject = new(expectedData);

        // Assert
        subject.IsSuccess.Should().BeTrue();
        subject.IsFailure.Should().BeFalse();
        subject.ResponseData.Should().Be(expectedData);
        subject.OuterException.Should().BeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithNullData_StoresNullData()
    {
        // Arrange & Act
        SuccessOperationResponse<string?> subject = new(null);

        // Assert
        subject.IsSuccess.Should().BeTrue();
        subject.ResponseData.Should().BeNull();
    }
}

[TestClass]
public sealed class FailureOperationResponseTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithException_SetsPropertiesCorrectly()
    {
        // Arrange
        BadRequestOperationException expectedException = new("Test error");

        // Act
        FailureOperationResponse<string> subject = new(expectedException);

        // Assert
        subject.IsSuccess.Should().BeFalse();
        subject.IsFailure.Should().BeTrue();
        subject.OuterException.Should().BeSameAs(expectedException);
        subject.ResponseData.Should().BeNull();
    }
}

[TestClass]
public sealed class OperationResponseTests
{
    private sealed class TestOperationResponse : OperationResponse<string>
    {
        public TestOperationResponse(string responseData) : base(responseData) => IsSuccess = true;

        public TestOperationResponse(OperationException ex) : base(ex) => IsSuccess = false;
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithResponseData_InitializesSuccessfully()
    {
        // Arrange
        string expectedData = "Test data";

        // Act
        TestOperationResponse subject = new(expectedData);

        // Assert
        subject.ResponseData.Should().Be(expectedData);
        subject.IsSuccess.Should().BeTrue();
        subject.IsFailure.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithException_InitializesForFailure()
    {
        // Arrange
        BadRequestOperationException expectedException = new("Test error");

        // Act
        TestOperationResponse subject = new(expectedException);

        // Assert
        subject.OuterException.Should().BeSameAs(expectedException);
        subject.IsSuccess.Should().BeFalse();
        subject.IsFailure.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsFailure_WhenIsSuccessTrue_ReturnsFalse()
    {
        // Arrange
        TestOperationResponse subject = new("data");

        // Act
        bool actual = subject.IsFailure;

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsFailure_WhenIsSuccessFalse_ReturnsTrue()
    {
        // Arrange
        TestOperationResponse subject = new(new BadRequestOperationException("error"));

        // Act
        bool actual = subject.IsFailure;

        // Assert
        actual.Should().BeTrue();
    }
}
