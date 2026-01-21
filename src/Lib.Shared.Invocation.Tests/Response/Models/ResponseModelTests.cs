using System.Net;
using Lib.Shared.Invocation.Response.Models;

namespace Lib.Shared.Invocation.Tests.Response.Models;

[TestClass]
public sealed class ResponseModelTests
{
    private sealed class TestResponseModel : ResponseModel
    {
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_SetsDefaultMetaData()
    {
        // Arrange & Act
        TestResponseModel subject = new();

        // Assert
        subject.MetaData.Should().NotBeNull();
        subject.Status.Should().BeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void Status_CanBeSet()
    {
        // Arrange
        TestResponseModel subject = new();
        StatusDataModel expectedStatus = new() { StatusCode = HttpStatusCode.OK, Message = "Success" };

        // Act
        subject.Status = expectedStatus;

        // Assert
        subject.Status.Should().BeSameAs(expectedStatus);
    }

    [TestMethod, TestCategory("unit")]
    public void MetaData_CanBeSet()
    {
        // Arrange
        TestResponseModel subject = new();
        MetaDataModel expectedMetaData = new() { ElapsedTime = "100ms", InvocationId = "test-id" };

        // Act
        subject.MetaData = expectedMetaData;

        // Assert
        subject.MetaData.Should().BeSameAs(expectedMetaData);
    }
}

[TestClass]
public sealed class FailureResponseModelTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_CreatesInstance()
    {
        // Arrange & Act
        FailureResponseModel subject = new();

        // Assert
        subject.Should().NotBeNull();
        subject.MetaData.Should().NotBeNull();
    }
}

[TestClass]
public sealed class SuccessResponseModelTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_CreatesInstance()
    {
        // Arrange & Act
        SuccessResponseModel subject = new();

        // Assert
        subject.Should().NotBeNull();
        subject.MetaData.Should().NotBeNull();
    }
}

[TestClass]
public sealed class SuccessDataResponseModelTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_CreatesInstanceWithDataProperty()
    {
        // Arrange & Act
        SuccessDataResponseModel<string> subject = new();

        // Assert
        subject.Should().NotBeNull();
        subject.Data.Should().BeNull();
        subject.MetaData.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void Data_CanBeSet()
    {
        // Arrange
        SuccessDataResponseModel<string> subject = new();
        string expectedData = "test data";

        // Act
        subject.Data = expectedData;

        // Assert
        subject.Data.Should().Be(expectedData);
    }
}

[TestClass]
public sealed class StatusDataModelTests
{
    [TestMethod, TestCategory("unit")]
    public void Properties_CanBeSet()
    {
        // Arrange
        StatusDataModel subject = new();
        HttpStatusCode expectedStatusCode = HttpStatusCode.Created;
        string expectedMessage = "Resource created";

        // Act
        subject.StatusCode = expectedStatusCode;
        subject.Message = expectedMessage;

        // Assert
        subject.StatusCode.Should().Be(expectedStatusCode);
        subject.Message.Should().Be(expectedMessage);
    }
}

[TestClass]
public sealed class MetaDataModelTests
{
    [TestMethod, TestCategory("unit")]
    public void TimeStamp_ReturnsCurrentUtcTime()
    {
        // Arrange & Act
        MetaDataModel subject = new();
        string timeStamp = subject.TimeStamp;

        // Assert
        timeStamp.Should().NotBeEmpty();
        DateTime parsedTime = DateTime.Parse(timeStamp).ToUniversalTime();
        double timeDifferenceSeconds = Math.Abs((DateTime.UtcNow - parsedTime).TotalSeconds);
        timeDifferenceSeconds.Should().BeLessThan(10); // Allow up to 10 seconds difference for system variations
    }

    [TestMethod, TestCategory("unit")]
    public void InvocationId_HasDefaultValue()
    {
        // Arrange & Act
        MetaDataModel subject = new();

        // Assert
        subject.InvocationId.Should().Be("not_provided");
    }

    [TestMethod, TestCategory("unit")]
    public void ElapsedTime_HasDefaultValue()
    {
        // Arrange & Act
        MetaDataModel subject = new();

        // Assert
        subject.ElapsedTime.Should().Be("not_provided");
    }

    [TestMethod, TestCategory("unit")]
    public void Properties_CanBeSet()
    {
        // Arrange
        MetaDataModel subject = new();
        string expectedInvocationId = "test-invocation-id";
        string expectedElapsedTime = "250ms";

        // Act
        subject.InvocationId = expectedInvocationId;
        subject.ElapsedTime = expectedElapsedTime;

        // Assert
        subject.InvocationId.Should().Be(expectedInvocationId);
        subject.ElapsedTime.Should().Be(expectedElapsedTime);
    }
}

[TestClass]
public sealed class UnhandledExceptionInternalServerErrorResponseModelTests
{
    private sealed class ExecutionContextFake : IExecutionContext
    {
        private readonly TimeSpan _elapsedTime;
        private readonly string _invocationId;

        public ExecutionContextFake(TimeSpan elapsedTime, string invocationId)
        {
            _elapsedTime = elapsedTime;
            _invocationId = invocationId;
        }

        public TimeSpan ElapsedTime() => _elapsedTime;
        public string InvocationId() => _invocationId;
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithExecutionContextAndException_SetsProperties()
    {
        // Arrange
        TimeSpan expectedElapsedTime = TimeSpan.FromMilliseconds(500);
        string expectedInvocationId = "test-invocation";
        ExecutionContextFake executionContext = new(expectedElapsedTime, expectedInvocationId);
        Exception expectedException = new InvalidOperationException("Test exception");

        // Act
        UnhandledExceptionInternalServerErrorResponseModel subject = new(executionContext, expectedException);

        // Assert
        subject.Status.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        subject.Status.Message.Should().Be("Test exception");
        subject.MetaData.ElapsedTime.Should().Be("00:00:00.5000000");
        subject.MetaData.InvocationId.Should().Be(expectedInvocationId);
    }
}

[TestClass]
public sealed class UnhandledExceptionBadRequestResponseModelTests
{
    private sealed class ExecutionContextFake : IExecutionContext
    {
        private readonly TimeSpan _elapsedTime;
        private readonly string _invocationId;

        public ExecutionContextFake(TimeSpan elapsedTime, string invocationId)
        {
            _elapsedTime = elapsedTime;
            _invocationId = invocationId;
        }

        public TimeSpan ElapsedTime() => _elapsedTime;
        public string InvocationId() => _invocationId;
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithExecutionContextAndException_SetsProperties()
    {
        // Arrange
        TimeSpan expectedElapsedTime = TimeSpan.FromMilliseconds(250);
        string expectedInvocationId = "bad-request-invocation";
        ExecutionContextFake executionContext = new(expectedElapsedTime, expectedInvocationId);
        Exception expectedException = new ArgumentException("Invalid argument");

        // Act
        UnhandledExceptionBadRequestResponseModel subject = new(executionContext, expectedException);

        // Assert
        subject.Status.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        subject.Status.Message.Should().Be("Invalid argument");
        subject.MetaData.ElapsedTime.Should().Be("00:00:00.2500000");
        subject.MetaData.InvocationId.Should().Be(expectedInvocationId);
    }
}
