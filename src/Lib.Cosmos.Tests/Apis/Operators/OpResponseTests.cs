using System.Net;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Cosmos.Tests.Apis.Operators;

[TestClass]
public class OpResponseTests
{
    private sealed class TestOpResponse : OpResponse<string>
    {
        private readonly string _value;
        private readonly HttpStatusCode _statusCode;

        public TestOpResponse(string value, HttpStatusCode statusCode)
        {
            _value = value;
            _statusCode = statusCode;
        }

        public override string Value => _value;
        public override HttpStatusCode StatusCode => _statusCode;
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ShouldReturnValue()
    {
        // Arrange
        const string Expected = "test-value";
        TestOpResponse subject = new(Expected, HttpStatusCode.OK);

        // Act
        string actual = subject.AsSystemType();

        // Assert
        _ = actual.Should().Be(Expected);
    }

    [TestMethod, TestCategory("unit")]
    public void ImplicitConversion_ShouldReturnValue()
    {
        // Arrange
        const string Expected = "test-value";
        TestOpResponse subject = new(Expected, HttpStatusCode.OK);

        // Act
        string actual = subject;

        // Assert
        _ = actual.Should().Be(Expected);
    }

    [TestMethod, TestCategory("unit")]
    public void IsSuccessful_ShouldReturnTrue_WhenStatusCodeIs200()
    {
        // Arrange
        TestOpResponse subject = new("value", HttpStatusCode.OK);

        // Act
        bool actual = subject.IsSuccessful();

        // Assert
        _ = actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsSuccessful_ShouldReturnTrue_WhenStatusCodeIs201()
    {
        // Arrange
        TestOpResponse subject = new("value", HttpStatusCode.Created);

        // Act
        bool actual = subject.IsSuccessful();

        // Assert
        _ = actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsSuccessful_ShouldReturnTrue_WhenStatusCodeIs299()
    {
        // Arrange
        TestOpResponse subject = new("value", (HttpStatusCode)299);

        // Act
        bool actual = subject.IsSuccessful();

        // Assert
        _ = actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsSuccessful_ShouldReturnFalse_WhenStatusCodeIs300()
    {
        // Arrange
        TestOpResponse subject = new("value", HttpStatusCode.MultipleChoices);

        // Act
        bool actual = subject.IsSuccessful();

        // Assert
        _ = actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsSuccessful_ShouldReturnFalse_WhenStatusCodeIs400()
    {
        // Arrange
        TestOpResponse subject = new("value", HttpStatusCode.BadRequest);

        // Act
        bool actual = subject.IsSuccessful();

        // Assert
        _ = actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsSuccessful_ShouldReturnFalse_WhenStatusCodeIs500()
    {
        // Arrange
        TestOpResponse subject = new("value", HttpStatusCode.InternalServerError);

        // Act
        bool actual = subject.IsSuccessful();

        // Assert
        _ = actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsNotSuccessful_ShouldReturnFalse_WhenStatusCodeIs200()
    {
        // Arrange
        TestOpResponse subject = new("value", HttpStatusCode.OK);

        // Act
        bool actual = subject.IsNotSuccessful();

        // Assert
        _ = actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsNotSuccessful_ShouldReturnTrue_WhenStatusCodeIs400()
    {
        // Arrange
        TestOpResponse subject = new("value", HttpStatusCode.BadRequest);

        // Act
        bool actual = subject.IsNotSuccessful();

        // Assert
        _ = actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsNotSuccessful_ShouldReturnTrue_WhenStatusCodeIs500()
    {
        // Arrange
        TestOpResponse subject = new("value", HttpStatusCode.InternalServerError);

        // Act
        bool actual = subject.IsNotSuccessful();

        // Assert
        _ = actual.Should().BeTrue();
    }
}
